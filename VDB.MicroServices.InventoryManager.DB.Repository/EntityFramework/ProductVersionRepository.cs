using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.Repository.EntityFramework
{
    public class ProductVersionRepository : EFSoftDeleteRepository<InventoryManagerDataContext, ProductVersion>
    {
        public ProductVersionRepository(InventoryManagerDataContext dataContext) : base(dataContext) { }
    }
}
