using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;

namespace VDB.MicroServices.CVEData.Manager.Business.Implementation
{
    public class CVEDataStorageBusinessManager : ICVEDataStorageBusinessManager
    {
        private readonly ICVEDataUnitOfWork DB;
        private readonly ICVEOperations CVEOperations;
        private readonly ICPEOperations CPEOperations;
        private readonly IProductBusinessManager ProductManager;
        private readonly ICVENodeOperations CVENodeOperations;
        private readonly CVEStorageSettings CVEStorageSettings;

        public CVEDataStorageBusinessManager(ICVEDataUnitOfWork db, ICVEOperations cveOperations, ICPEOperations cpeOperations, ICVENodeOperations cveNodeOperations, IProductBusinessManager productManager, IOptions<CVEStorageSettings> cveStorageSettings)
        {
            this.DB = db;
            this.CVEOperations = cveOperations;
            this.ProductManager = productManager;
            this.CPEOperations = cpeOperations;
            this.CVENodeOperations = cveNodeOperations;
            this.CVEStorageSettings = cveStorageSettings.Value;
        }

        public async Task StoreCVEResult(Model.DTO.CVEData.CVEResult result)
        {
            foreach (Model.DTO.CVEData.CVE cveResult in result.CVEs)
            {
                if (cveResult.Nodes.IsNullOrEmpty())
                {
                    continue;
                }

                if (this.DB.GetTrackedEntityCount() > this.CVEStorageSettings.ChangeTrackerCountCommitThreshold)
                {
                    await this.DB.SaveAsync();
                    this.DB.ClearChangeTrakcer();
                }

                Model.Entity.POCO.CVE existingCVE = await this.CVEOperations.GetCVEByCVEId(cveResult.ID, i => i.Nodes.SelectMany(n => n.CPEs).Select(c => c.Product).Select(p => p.Vendor));
                if (existingCVE == null)
                {
                    CreateNewCVE(cveResult);
                }
                else
                {
                    if (existingCVE.LastModifiedDate.ToUniversalTime() != cveResult.LastModifiedDate.ToUniversalTime())
                    {
                        CompareAndUpdateCVE(existingCVE, cveResult);
                    }
                }
            }
            
            await this.DB.SaveAsync();
        }

        private void CreateNewCVE(Model.DTO.CVEData.CVE cveResult)
        {
            Model.Entity.POCO.CVE newCVE = new()
            {
                CVEID = cveResult.ID,
                Description = cveResult.Description,
                PublishedDate = cveResult.PublishedDate,
                LastModifiedDate = cveResult.LastModifiedDate                
            };
            newCVE.Nodes = cveResult.Nodes?.Select(node => MapNode(newCVE, node, null))?.Where(node => node != null)?.ToList();

            this.CVEOperations.Create(newCVE);
        }

        private Model.Entity.POCO.CVENode MapNode(Model.Entity.POCO.CVE cve, Model.DTO.CVEData.CVENode nodeResult, Model.Entity.POCO.CVENode parentNode)
        {
            if (nodeResult == null) return null;

            Model.Entity.POCO.CVENode node = new()
            {
                CVE = cve,
                ParentNode = parentNode,
                ParentNodeId = parentNode?.Id,
                Operator = nodeResult.Operator,
                CPEs = nodeResult.CPEs?.Select(cpe => MapCPE(cpe))?.Where(cpe => cpe != null)?.ToList()
            };
            node.ChildrenNodes = nodeResult.Children?.Select(childrenNode => MapNode(cve, childrenNode, node))?.Where(childrenNode => childrenNode != null)?.ToList();        

            return node;
        }

        private Model.Entity.POCO.CPE MapCPE(Model.DTO.CVEData.CPE cpeResult)
        {
            if (cpeResult == null) return null;

            string[] splittedCPE = cpeResult.URI.Split(":");
            string vendorName = splittedCPE[3].Replace("_", " ").Trim();
            string productName = splittedCPE[4].Replace("_", " ").Trim();
            string version = splittedCPE[5].Trim();

            Model.Entity.POCO.Product product = this.ProductManager.GetOrCreateProduct(productName, vendorName).Result;

            Model.Entity.POCO.CPE cpe = this.CPEOperations.GetCPEByProductAndVersion(product.Id, version, cpeResult.VersionStartIncluding, cpeResult.VersionEndIncluding, cpeResult.VersionStartExcluding, cpeResult.VersionEndExcluding).Result;

            if (cpe != null)
            {
                return cpe;
            }

            cpe = new()
            {
                IsVulnerable = cpeResult.IsVulnerable,
                URI = cpeResult.URI,
                VersionStartIncluding = cpeResult.VersionStartIncluding,
                VersionEndIncluding = cpeResult.VersionEndIncluding,
                VersionStartExcluding = cpeResult.VersionStartExcluding,
                VersionEndExcluding = cpeResult.VersionEndExcluding,
                SpecificVersion = version,
                Product = product
            };

            this.CPEOperations.Create(cpe);

            return cpe;
        }

        private void CompareAndUpdateCVE(Model.Entity.POCO.CVE existingCVE, Model.DTO.CVEData.CVE cveResult)
        {
            existingCVE.LastModifiedDate = cveResult.LastModifiedDate;
            existingCVE.Description = cveResult.Description;

            if (existingCVE.Nodes.HasElements())
            {
                existingCVE.Nodes.ForEach(node => this.CVENodeOperations.Delete(node));
            }
            if (cveResult.Nodes.HasElements())
            {
                existingCVE.Nodes ??= new List<Model.Entity.POCO.CVENode>();
                existingCVE.Nodes.AddRange(cveResult.Nodes?.Select(node => MapNode(existingCVE, node, null))?.Where(node => node != null));
            }

            this.CVEOperations.Update(existingCVE);
        }
    }
}
