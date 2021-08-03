using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model.Derived.Validation;
using VDB.Architecture.Concern.CustomAttributes;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.Resources.ResourceKeys;

namespace VDB.Architecture.Concern.GenericValidator
{
    public class Validator
    {
        private readonly IDatabase Cache;
        
        private readonly string DateFormat = "yyyy.MM.dd HH:mm:ss z";

        public Validator(IDatabase cache)
        {
            this.Cache = cache;
        }

        public void Validate<T>(T obj)
        {
            if ((typeof(T).CustomAttributes?.Any(ca => ca.AttributeType == typeof(NotValidatedAttribute))).GetValueOrDefault()) return;

            string @namespace = typeof(T).Namespace;
            string className = typeof(T).Name;
            string displayName = typeof(T).GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? className;

            ConcurrentBag<ValidationExceptionMessage> errors = new();

            string settingString = this.Cache.StringGet($"ValidationSettings:{@namespace}:{className}");
            if (String.IsNullOrWhiteSpace(settingString))
            {
                return;
            }

            ObjectValidationSetting settingObject = settingString.DeserializeJSON<ObjectValidationSetting>();

            if (obj == null)
            {
                bool isNullAllowed = settingObject.IsNullAllowed;
                if (isNullAllowed) return;

                errors.Add(new ValidationExceptionMessage(ErrorResourceKeys.FieldCannotBeEmpty, displayName));
            }
            else
            {
                PropertyInfo[] properties = obj.GetType().GetProperties();
                if (properties.IsNullOrEmpty()) return;

                Parallel.ForEach(properties, property =>
                {
                    errors.AddRangeSafe(ValidateProperty(settingObject.PropertyValidations?.FirstOrDefault(pv => pv.PropertyName == property.Name), property, obj));
                });
            }

            if (errors.HasElements())
            {
                throw new ValidationException(errors.ToArray());
            }
        }

        private ConcurrentBag<ValidationExceptionMessage> ValidateProperty(PropertyValidationSetting validationSetting, PropertyInfo property, object containerObject)
        {
            if (validationSetting == null || (property.CustomAttributes?.Any(ca => ca.AttributeType == typeof(NotValidatedAttribute))).GetValueOrDefault())
            {
                return null;
            }

            string @namespace = property.DeclaringType.Namespace;
            string className = property.DeclaringType.Name;
            string propertyName = property.Name;
            string displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? propertyName;
            object value = property.GetValue(containerObject);

            if (property.PropertyType.Namespace == "System" && (value as IEnumerable<object>) == null)
            {
                return ValidateProperty(validationSetting, value, property.PropertyType, displayName);
            }
            else
            {
                ConcurrentBag<ValidationExceptionMessage> errors = new();

                var collection = value as IEnumerable<object>;
                if (value == null && collection?.Count() == 0)
                {
                    if (validationSetting.IsRequired)
                    {
                        errors.Add(new ValidationExceptionMessage(ErrorResourceKeys.FieldCannotBeEmpty, displayName));
                    }
                    return errors;
                }

                if (collection != null)
                {
                    if (!collection.Any()) return errors;

                    Parallel.ForEach(collection, item =>
                    {
                        if (item.GetType().Namespace == "System")
                        {
                            errors.AddRangeSafe(ValidateProperty(validationSetting, item, item.GetType(), displayName));
                        }
                        else
                        {
                            PropertyInfo[] subProperties = item?.GetType()?.GetProperties();
                            if (subProperties.HasElements())
                            {
                                Parallel.ForEach(subProperties, subProperty =>
                                {
                                    errors.AddRangeSafe(ValidateProperty(validationSetting, subProperty, item));
                                });
                            }
                        }
                    });
                }
                else
                {
                    PropertyInfo[] subProperties = property.PropertyType.IsInterface ? value.GetType().GetProperties() : property.PropertyType.GetProperties();
                    if (subProperties == null) return errors;

                    Parallel.ForEach(subProperties, subProperty =>
                    {
                        errors.AddRangeSafe(ValidateProperty(validationSetting, subProperty, value));
                    });
                }

                return errors;
            }
        }

        private ConcurrentBag<ValidationExceptionMessage> ValidateProperty(PropertyValidationSetting settings, object value, Type propertyTpe, string displayName)
        {
            ConcurrentBag<ValidationExceptionMessage> errors = new();

            if (settings.IsRequired && ((value == null) || (propertyTpe == typeof(string) && String.IsNullOrWhiteSpace(value?.ToString())) || (propertyTpe == typeof(Guid) && (Guid)value == default(Guid))))
            {
                errors.Add(new ValidationExceptionMessage(ErrorResourceKeys.FieldCannotBeEmpty, displayName));
                return errors;
            }

            if (value == null) return errors;

            bool hasMaxValueError = false;
            bool hasMinValueError = false;
            bool lengthErrorFlag = false;            

            switch (value)
            {
                case var stringValue when stringValue is string || stringValue is Guid:
                    lengthErrorFlag = true;
                    hasMaxValueError = !String.IsNullOrWhiteSpace(settings.MaxValue) && value.ToString().Length > Convert.ToInt32(settings.MaxValue);
                    hasMinValueError = !String.IsNullOrWhiteSpace(settings.MinValue) && value.ToString().Length < Convert.ToInt32(settings.MinValue);
                    break;
                case DateTime _:
                    hasMaxValueError = !String.IsNullOrWhiteSpace(settings.MaxValue) && ((DateTime)Convert.ChangeType(value, typeof(DateTime))).ToUniversalTime() > DateTime.ParseExact(settings.MaxValue, this.DateFormat, null);
                    hasMinValueError = !String.IsNullOrWhiteSpace(settings.MinValue) && ((DateTime)Convert.ChangeType(value, typeof(DateTime))).ToUniversalTime() < DateTime.ParseExact(settings.MinValue, this.DateFormat, null);
                    break;
                case var numericValue when numericValue is int || numericValue is short || numericValue is long || numericValue is float || numericValue is double || numericValue is int || numericValue is decimal:
                    hasMaxValueError = !String.IsNullOrWhiteSpace(settings.MaxValue) && Convert.ToDecimal(numericValue) > Convert.ToDecimal(settings.MaxValue);
                    hasMinValueError = !String.IsNullOrWhiteSpace(settings.MinValue) && Convert.ToDecimal(numericValue) < Convert.ToDecimal(settings.MinValue);
                    break;
                case bool _:
                    break;
                default:
                    throw new NotImplementedException($"{value.GetType().Name} İçin Validator Bulunamadı.");
            }

            if (hasMaxValueError)
            {
                errors.Add(new ValidationExceptionMessage(lengthErrorFlag ? ErrorResourceKeys.InvalidMaxLength : ErrorResourceKeys.InvalidMaxValue, displayName, maxValue: settings.MaxValue));
            }
            else if (hasMinValueError)
            {
                errors.Add(new ValidationExceptionMessage(lengthErrorFlag ? ErrorResourceKeys.InvalidMinLength : ErrorResourceKeys.InvalidMinValue, displayName, minValue: settings.MinValue));
            }

            if (settings.AcceptedValues.HasElements() && !settings.AcceptedValues.Contains(value.ToString()))
            {
                errors.Add(new ValidationExceptionMessage(ErrorResourceKeys.InvalidFieldValue, displayName));
            }

            return errors;
        }
    }
}
