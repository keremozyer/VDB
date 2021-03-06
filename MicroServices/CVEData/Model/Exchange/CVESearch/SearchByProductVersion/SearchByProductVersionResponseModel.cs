using System;
using System.Collections.Generic;

namespace VDB.MicroServices.CVEData.Model.Exchange.CVESearch.SearchByProductVersion
{
    public record SearchByProductVersionResponseModel
    {
        public List<CVEData> CVEs { get; set; }
    }

    public record CVEData
    {
        public string CVEID { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Description { get; set; }
        public CVEMatchType? MatchType { get; set; }
        public IEnumerable<ProductData> AdditionalRequiredProducts { get; set; }
    }

    public enum CVEMatchType
    {
        SpecificVersion = 1,
        NumericVersionRange = 2,
        NonNumericVersionRange = 3,
        WithoutVersion = 4,
        ProductWithoutVersion = 5
    }

    public record ProductData
    {
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public string SpecificVersion { get; set; }
        public string VersionRangeStart { get; set; }
        public bool IsStartingVersionInclusive { get; set; }
        public string VersionRangeEnd { get; set; }
        public bool IsEndingVersionInclusive { get; set; }
    }
}
