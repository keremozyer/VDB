using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.Repository.EntityFramework
{
    public class ServerRepository : EFSoftDeleteRepository<InventoryManagerDataContext, Server>
    {
        public ServerRepository(InventoryManagerDataContext dataContext) : base(dataContext) { }
    }
}
