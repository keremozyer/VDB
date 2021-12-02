using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface ICVEOperations
    {
        public Task<CVE> GetCVEByCVEId(string cveId, params Expression<Func<CVE, object>>[] includes);
        public void Create(CVE cve);
        void Update(CVE existingCVE);        
    }
}
