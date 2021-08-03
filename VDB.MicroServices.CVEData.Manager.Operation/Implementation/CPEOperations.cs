using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class CPEOperations : ICPEOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public CPEOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(CPE cpe)
        {
            this.DB.CPERepository.Create(cpe);
        }

        public async Task<CPE> GetCPEByProductAndVersion(Guid productId, string specificVersion, string versionStartIncluding, string versionEndIncluding, string versionStartExcluding, string versionEndExcluding)
        {
            return await this.DB.CPERepository.GetFirstAsync(c => c.ProductId == productId && c.SpecificVersion == specificVersion && c.VersionStartIncluding == versionStartIncluding && c.VersionEndIncluding == versionEndIncluding && c.VersionStartExcluding == versionStartExcluding && c.VersionEndExcluding == versionEndExcluding);
        }

        public async Task<List<CPE>> GetVulnerableCPEsByProductAndVendorName(string productName, string vendorName, params Expression<Func<CPE, object>>[] includes)
        {
            return await this.DB.CPERepository.GetAsync(c => c.IsVulnerable && c.Product.Name == productName && c.Product.Vendor.Name == vendorName, includes: includes);
        }
    }
}
