using System.Collections.Generic;

namespace VDB.Architecture.Concern.GenericValidator
{
    public class ObjectValidationSetting
    {
        public bool IsNullAllowed { get; set; }
        public IEnumerable<PropertyValidationSetting> PropertyValidations { get; set; }
    }
}
