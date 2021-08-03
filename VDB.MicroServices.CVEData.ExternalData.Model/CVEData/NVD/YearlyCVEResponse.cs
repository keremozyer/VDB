using System;
using System.Collections.Generic;

namespace VDB.MicroServices.CVEData.ExternalData.Model.CVEData.NVD
{
    public record YearlyCVEResponse
    {
        public DateTime CVE_data_timestamp { get; set; }
        public int CVE_data_numberOfCVEs { get; set; }
        public List<CVEItem> CVE_Items { get; set; }
    }
}
