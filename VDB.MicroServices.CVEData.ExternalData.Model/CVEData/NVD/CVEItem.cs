using System;
using System.Collections.Generic;

namespace VDB.MicroServices.CVEData.ExternalData.Model.CVEData.NVD
{
    public record CVEItem
    {
        public CVEData CVE { get; set; }
        public CVEConfiguration Configurations { get; set; }        
        public DateTime publishedDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
    }

    public record CVEData
    {
        public CVEMetaData CVE_data_meta { get; set; }
        public CVEDescription Description { get; set; }
    }

    public record CVEMetaData
    {
        public string ID { get; set; }
    }

    public record CVEDescription
    {
        public List<CVEDescriptionData> description_data { get; set; }
    }

    public record CVEDescriptionData
    {
        public string value { get; set; }
    }

    public record CVEConfiguration
    {
        public List<CVENode> nodes { get; set; }
    }

    public record CVENode
    {
        public List<CVENode> children { get; set; }
        public string @operator { get; set; }
        public List<CPEMatch> cpe_match { get; set; }
    }

    public record CPEMatch
    {
        public bool vulnerable { get; set; }
        public string cpe23Uri { get; set; }
        public List<CPEName> cpe_name { get; set; }
        public string versionStartIncluding { get; set; }
        public string versionStartExcluding { get; set; }
        public string versionEndIncluding { get; set; }
        public string versionEndExcluding { get; set; }
    }

    public record CPEName
    {
        public string cpe23Uri { get; set; }
    }
}
