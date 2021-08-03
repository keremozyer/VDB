using System;
using System.Collections.Generic;

namespace VDB.MicroServices.CVEData.Model.DTO.CVEData
{
    public record CVEResult
    {
        public int TotalCVECount { get; set; }
        public int PageStartIndex { get; set; }
        public DateTime SearchTimestamp { get; set; }
        public List<CVE> CVEs { get; set; }
    }

    public record CVE
    {
        public string ID { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }        
        public string Description { get; set; }
        public List<CVENode> Nodes { get; set; }
    }

    public record CVENode
    {
        public List<CVENode> Children { get; set; }
        public string Operator { get; set; }
        public List<CPE> CPEs { get; set; }
    }

    public record CPE
    {
        public bool IsVulnerable { get; set; }
        public string URI { get; set; }
        public string VersionStartIncluding { get; set; }
        public string VersionStartExcluding { get; set; }
        public string VersionEndIncluding { get; set; }
        public string VersionEndExcluding { get; set; }
    }
}
