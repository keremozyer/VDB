using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using System.Reflection;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Context.Extensions
{
    public static class MutableEntityTypeExtensions
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityType)
        {
            var methodToCall = typeof(MutableEntityTypeExtensions).GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(entityType.ClrType);
            var filter = methodToCall.Invoke(null, Array.Empty<object>());
            entityType.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetSoftDeleteFilter<Entity>() where Entity : SoftDeletedEntity
        {
            Expression<Func<Entity, bool>> filter = x => x.DeletedAt == null;
            return filter;
        }
    }
}
