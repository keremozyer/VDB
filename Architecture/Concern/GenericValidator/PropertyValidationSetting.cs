using System.Collections.Generic;

namespace VDB.Architecture.Concern.GenericValidator
{
    public class PropertyValidationSetting
    {
        public string PropertyName { get; set; }
        public bool IsRequired { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public IEnumerable<string> AcceptedValues { get; set; }
    }
}
