using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface ICVENodeOperations
    {
        void Delete(CVENode node);
        Task<List<CVENode>> GetAllNodesOfParent(Guid key, params Expression<Func<CVENode, object>>[] includes);
    }
}
