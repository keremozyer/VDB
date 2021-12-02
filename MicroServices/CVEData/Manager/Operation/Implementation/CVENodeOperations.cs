using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class CVENodeOperations : ICVENodeOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public CVENodeOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Delete(CVENode node)
        {
            this.DB.CVENodeRepository.Delete(node);
        }

        public Task<List<CVENode>> GetAllNodesOfParent(Guid parentNodeID, params Expression<Func<CVENode, object>>[] includes)
        {
            return this.DB.CVENodeRepository.GetAsync(n => n.ParentNodeId == parentNodeID, includes: includes);
        }
    }
}
