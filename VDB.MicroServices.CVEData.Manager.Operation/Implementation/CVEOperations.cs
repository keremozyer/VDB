using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class CVEOperations : ICVEOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public CVEOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(CVE cve)
        {
            this.DB.CVERepository.Create(cve);
        }

        public void Update(CVE cve)
        {
            this.DB.CVERepository.Update(cve);
        }

        public async Task<CVE> GetCVEByCVEId(string cveId, params Expression<Func<CVE, object>>[] includes)
        {
            return await this.DB.CVERepository.GetFirstAsync(c => c.CVEID == cveId, includes: includes);
        }
    }
}
