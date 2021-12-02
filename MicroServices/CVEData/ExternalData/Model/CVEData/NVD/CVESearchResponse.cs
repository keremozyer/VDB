using System;
using System.Collections.Generic;

namespace VDB.MicroServices.CVEData.ExternalData.Model.CVEData.NVD
{
    public record CVESearchResponse
    {
        public int startIndex { get; set; }
        public int totalResults { get; set; }
        public CVESearchResult result { get; set; }
    }

    public record CVESearchResult
    {
        public DateTime CVE_data_timestamp { get; set; }
        public List<CVEItem> CVE_Items { get; set; }
    }
}
