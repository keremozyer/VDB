using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface ICPEOperations
    {
        void Create(CPE cpe);
        public Task<CPE> GetCPEByProductAndVersion(Guid productId, string specificVersion, string versionStartIncluding, string versionEndIncluding, string versionStartExcluding, string versionEndExcluding);
        Task<List<CPE>> GetVulnerableCPEsByProductAndVendorName(string productName, string vendorName, params Expression<Func<CPE, object>>[] includes);
    }
}
