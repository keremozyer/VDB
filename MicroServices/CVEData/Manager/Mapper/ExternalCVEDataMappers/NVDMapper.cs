using System.Collections.Generic;
using System.Linq;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.CVEData.ExternalData.Model.CVEData.NVD;
using VDB.MicroServices.CVEData.Model.DTO.CVEData;

namespace VDB.MicroServices.CVEData.Manager.Mapper.ExternalCVEDataMappers
{
    public static class NVDMapper
    {
        public static CVEResult MapYearlyResult(YearlyCVEResponse response)
        {
            return new()
            {
                SearchTimestamp = response.CVE_data_timestamp,
                CVEs = response.CVE_Items?.Select(c => MapCVE(c))?.Where(c => c != null)?.ToList()
            };
        }

        public static CVEResult MapSearchResult(CVESearchResponse response)
        {
            if (response?.result == null) return null;

            return new()
            {
                TotalCVECount = response.totalResults,
                PageStartIndex = response.startIndex,
                SearchTimestamp = response.result.CVE_data_timestamp,
                CVEs = response.result.CVE_Items?.Select(ci => MapCVE(ci))?.Where(c => c != null)?.ToList()
            };
        }

        private static CVE MapCVE(CVEItem cve)
        {
            if (cve == null) return null;

            return new()
            {
                ID = cve.CVE.CVE_data_meta.ID,
                Description = cve.CVE.Description?.description_data?.FirstOrDefault()?.value,
                PublishedDate = cve.publishedDate,
                LastModifiedDate = cve.lastModifiedDate,
                Nodes = cve.Configurations?.nodes?.Select(n => MapNode(n))?.Where(n => n != null)?.ToList()
            };
        }

        private static Model.DTO.CVEData.CVENode MapNode(ExternalData.Model.CVEData.NVD.CVENode node)
        {
            if (node == null) return null;

            return new()
            {
                Operator = node.@operator,
                Children = node.children?.Select(c => MapNode(c))?.Where(c => c != null)?.ToList(),
                CPEs = MapCPEs(node.cpe_match)
            };
        }

        private static List<CPE> MapCPEs(List<CPEMatch> cpeMatches)
        {
            if (cpeMatches.IsNullOrEmpty()) return null;

            List<CPE> cpes = new();

            foreach (CPEMatch cpeMatch in cpeMatches)
            {
                if (cpeMatch.cpe_name.HasElements())
                {
                    cpes.AddRange(cpeMatch.cpe_name.Select(cn => new CPE() 
                    { 
                        IsVulnerable = cpeMatch.vulnerable, 
                        URI = cn.cpe23Uri,
                        VersionEndExcluding = cpeMatch.versionEndExcluding,
                        VersionEndIncluding = cpeMatch.versionEndIncluding,
                        VersionStartExcluding = cpeMatch.versionStartExcluding,
                        VersionStartIncluding = cpeMatch.versionStartIncluding
                    }));
                }
                else
                {
                    cpes.Add(new CPE() 
                    { 
                        IsVulnerable = cpeMatch.vulnerable, 
                        URI = cpeMatch.cpe23Uri,
                        VersionEndExcluding = cpeMatch.versionEndExcluding,
                        VersionEndIncluding = cpeMatch.versionEndIncluding,
                        VersionStartExcluding = cpeMatch.versionStartExcluding,
                        VersionStartIncluding = cpeMatch.versionStartIncluding
                    });
                }
            }

            return cpes;
        }
    }
}
