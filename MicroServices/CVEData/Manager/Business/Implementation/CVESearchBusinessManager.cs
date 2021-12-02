using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.Helper;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;
using VDB.MicroServices.CVEData.Model.Exchange.CVESearch.SearchByProductVersion;

namespace VDB.MicroServices.CVEData.Manager.Business.Implementation
{
    public class CVESearchBusinessManager : ICVESearchBusinessManager
    {
        private readonly CVESearchSettings CVESearchSettings;
        private readonly ICPEOperations CPEOperations;
        private readonly ICVENodeOperations CVENodeOperations;
        private readonly IMapper Mapper;

        public CVESearchBusinessManager(IOptions<CVESearchSettings> cveSearchSettings, ICPEOperations cpeOperations, ICVENodeOperations cveNodeOperations, IMapper mapper)
        {
            this.CVESearchSettings = cveSearchSettings.Value;
            this.CPEOperations = cpeOperations;
            this.CVENodeOperations = cveNodeOperations;
            this.Mapper = mapper;
        }

        public async Task<SearchByProductVersionResponseModel> SearchByProductVersion(SearchByProductVersionRequestModel request)
        {
            List<CPE> cpes = await this.CPEOperations.GetVulnerableCPEsByProductAndVendorName(request.ProductName, request.VendorName, c => c.CVENodes.Select(n => n.CVE), c => c.CVENodes.Select(n => n.ParentNode), c => c.CVENodes.SelectMany(n => n.CPEs).Select(c => c.Product.Vendor));
            if (cpes.IsNullOrEmpty())
            {
                return null;
            }

            bool isVersionAllDigit = request.ProductVersion.IsAllDigit(".");

            IEnumerable<CPE> cpesWithNodes = cpes.Where(c => c.CVENodes.HasElements());
            IEnumerable<CPE> cpesWithSpecificVersion = cpesWithNodes.Where(c => c.SpecificVersion == request.ProductVersion);
            IEnumerable<CPE> cpesWithNonNumericVersionRange = cpesWithNodes.ExceptSafe(cpesWithSpecificVersion)?.Where(c => c.HasNonNumericVersionRange(CVESearchSettings.StandartVersionSeperator));
            IEnumerable<CPE> cpesWithNumericVersionRange = cpesWithNodes.ExceptSafe(cpesWithSpecificVersion)?.Where(c => c.HasNumericVersionRange(CVESearchSettings.StandartVersionSeperator));
            IEnumerable<CPE> cpesWithMatchedVersionRanges = isVersionAllDigit ? cpesWithNumericVersionRange?.Where(c => VersionHelpers.DoesVersionMatchRange(request.ProductVersion, c.VersionRangeStart, c.VersionRangeEnd, c.IsStartingVersionInclusive, c.IsEndingVersionInclusive, this.CVESearchSettings.StandartVersionSeperator, this.CVESearchSettings.StandartVersionNumber)) : cpesWithNumericVersionRange;
            IEnumerable<CPE> cpesWithoutVersionData = cpesWithNodes?.Where(c => (this.CVESearchSettings.AllVersionsIndicators?.Contains(c.SpecificVersion)).GetValueOrDefault())?.ExceptSafe(cpesWithSpecificVersion)?.ExceptSafe(cpesWithNonNumericVersionRange)?.ExceptSafe(cpesWithNumericVersionRange);

            SearchByProductVersionResponseModel response = new() { CVEs = new List<Model.Exchange.CVESearch.SearchByProductVersion.CVEData>() };

            foreach (CPE cpe in cpesWithSpecificVersion.ConcatSafe(cpesWithNonNumericVersionRange).ConcatSafe(cpesWithMatchedVersionRanges).ConcatSafe(cpesWithoutVersionData) ?? new List<CPE>())
            {
                IEnumerable<IGrouping<Guid, CVENode>> nodesGroupedByCVE = cpe.CVENodes.GroupBy(n => n.CVE.Id);
                foreach (IGrouping<Guid, CVENode> nodeGroup in nodesGroupedByCVE)
                {
                    // For now CPE data contains only 2 levels of node->node relation so node traversing is done in a foreach for ease of implementation. Must change to a recursive traverser in case data becomes n-level.
                    IEnumerable<IGrouping<Guid?, CVENode>> nodesGroupedByParent = nodeGroup.GroupBy(n => n.ParentNodeId);
                    foreach (IGrouping<Guid?, CVENode> parentGroup in nodesGroupedByParent)
                    {
                        if (parentGroup.Key != null && parentGroup.First().ParentNode.Operator != this.CVESearchSettings.NodeOperatorAnd)
                        {
                            // In this case node won't have any cpe attached to it.
                            continue;
                        }

                        var cveData = this.Mapper.Map<CVE, Model.Exchange.CVESearch.SearchByProductVersion.CVEData>(nodeGroup.First().CVE);
                        cveData.MatchType = DetermineCPEMatchType(cpe, isVersionAllDigit, cpesWithSpecificVersion, cpesWithNonNumericVersionRange, cpesWithMatchedVersionRanges, cpesWithoutVersionData);
                        response.CVEs.Add(cveData);
                        
                        if (parentGroup.Key != null)
                        {
                            List<CVENode> allNodesOfGroup = await this.CVENodeOperations.GetAllNodesOfParent(parentGroup.Key.Value, i => i.CPEs.Select(c => c.Product.Vendor));
                            cveData.AdditionalRequiredProducts = allNodesOfGroup.Where(n => n.CPEs.HasElements() && !(n.CPEs.Any(c => c.Id == cpe.Id)))?.SelectMany(n => n.CPEs)?.Select(c => this.Mapper.Map<CPE, ProductData>(c));
                        }
                        else if (parentGroup.First().Operator == this.CVESearchSettings.NodeOperatorAnd)
                        {
                            cveData.AdditionalRequiredProducts = parentGroup.First().CPEs?.Where(c => !(c.Id == cpe.Id))?.Select(c => this.Mapper.Map<CPE, ProductData>(c));
                        }
                    }
                }
            }

            response.CVEs = DistinctifyByIDBasedOnWeight(response.CVEs);

            return response;
        }

        private static CVEMatchType? DetermineCPEMatchType(CPE cpe, bool isVersionAllDigit, IEnumerable<CPE> cpesWithSpecificVersion, IEnumerable<CPE> cpesWithNonNumericVersionRange, IEnumerable<CPE> cpesWithMatchedVersionRanges, IEnumerable<CPE> cpesWithoutVersionData)
        {
            if ((cpesWithSpecificVersion?.Contains(cpe)).GetValueOrDefault()) return CVEMatchType.SpecificVersion;
            if ((cpesWithNonNumericVersionRange?.Contains(cpe)).GetValueOrDefault()) return CVEMatchType.NonNumericVersionRange;
            if ((cpesWithMatchedVersionRanges?.Contains(cpe)).GetValueOrDefault())
            {
                return isVersionAllDigit ? CVEMatchType.NumericVersionRange : CVEMatchType.ProductWithoutVersion;
            }
            if ((cpesWithoutVersionData?.Contains(cpe)).GetValueOrDefault()) return CVEMatchType.WithoutVersion;

            return null;
        }

        private List<Model.Exchange.CVESearch.SearchByProductVersion.CVEData> DistinctifyByIDBasedOnWeight(List<Model.Exchange.CVESearch.SearchByProductVersion.CVEData> cveData)
        {
            if (cveData.IsNullOrEmpty()) return cveData;

            List<Model.Exchange.CVESearch.SearchByProductVersion.CVEData> distinctList = new();

            var groupedData = cveData.GroupBy(c => c.CVEID);
            distinctList = groupedData.Where(g => g.Count() == 1)?.Where(g => g != null)?.SelectMany(g => g)?.ToList() ?? new();

            foreach (var group in groupedData.Where(g => g.Count() > 1) ?? new List<IGrouping<string, Model.Exchange.CVESearch.SearchByProductVersion.CVEData>>())
            {
                CVEMatchType? matchTypeToUse = group.Min(c => c.MatchType);
                var matchTypeFilteredGroup = group.Where(g => g.MatchType == matchTypeToUse);

                if (matchTypeFilteredGroup.Count() > 1)
                {
                    var groupWithoutAdditionalProduct = matchTypeFilteredGroup.FirstOrDefault(g => g.AdditionalRequiredProducts.IsNullOrEmpty());
                    if (groupWithoutAdditionalProduct != null)
                    {
                        distinctList.Add(groupWithoutAdditionalProduct);
                    }
                }
                else
                {
                    distinctList.AddRange(matchTypeFilteredGroup);
                }
            }

            return distinctList;
        }
    }
}
