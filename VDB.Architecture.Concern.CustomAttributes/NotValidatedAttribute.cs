using System;

namespace VDB.Architecture.Concern.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Struct)]
    public class NotValidatedAttribute : Attribute
    {
    }
}
