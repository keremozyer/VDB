using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VDB.Architecture.Concern.ExtensionMethods;

namespace VDB.Architecture.Concern.Helper.PredicateBuilding
{
    public static class PredicateBuilder
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethods().Where(x => x.Name == "Contains" && x.GetParameters()?.Count() == 1 && x.GetParameters().First().ParameterType == typeof(string)).First();
        private static readonly MethodInfo InMethod = typeof(List<string>).GetMethod("Contains");
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        public static Expression<Func<T, bool>> MakePredicate<T>(IEnumerable<ExpressionModel> filters)
        {
            if (filters == null) return null;

            filters = filters.Where(filter => filter != null);
            if (filters.IsNullOrEmpty()) return null;

            ParameterExpression parameterExpression = GetParameterExpression(typeof(T));
            var body = filters.Select(filter => MakePredicate(parameterExpression, filter)).Aggregate(Expression.AndAlso);
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameterExpression);

            return predicate;
        }

        private static Expression MakePredicate(ParameterExpression item, ExpressionModel filter)
        {
            Expression member = Expression.Property(item, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            member = ApplyMemberFunctionCalls(member, filter.MemberFunctions);

            return filter.LogicOperator switch
            {
                ExpressionOperators.Equals => Expression.Equal(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.NotEquals => Expression.NotEqual(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.GreaterThan => Expression.GreaterThan(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.LessThan => Expression.LessThan(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.LessThanOrEqual => Expression.LessThanOrEqual(member, Expression.Convert(constant, member.Type)),
                ExpressionOperators.Contains => Expression.Call(member, ContainsMethod, constant),
                ExpressionOperators.NotContains => Expression.Not(Expression.Call(member, ContainsMethod, constant)),
                ExpressionOperators.StartsWith => Expression.Call(member, StartsWithMethod, constant),
                ExpressionOperators.EndsWith => Expression.Call(member, EndsWithMethod, constant),
                ExpressionOperators.InMethod => Expression.Call(constant, InMethod, member),
                _ => null
            };
        }

        private static Expression ApplyMemberFunctionCalls(Expression member, IEnumerable<ExpressionMemberFunctions> memberFunctions)
        {
            if (memberFunctions.IsNullOrEmpty()) return member;

            foreach (ExpressionMemberFunctions function in memberFunctions)
            {
                member = Expression.Call(member, Enum.GetName(typeof(ExpressionMemberFunctions), function), null);
            }

            return member;
        }

        private static ParameterExpression GetParameterExpression(Type type)
        {
            return Expression.Parameter(type, type.Name);
        }
    }
}
