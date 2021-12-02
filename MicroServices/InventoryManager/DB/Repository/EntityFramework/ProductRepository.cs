using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.Repository.EntityFramework
{
    public class ProductRepository : EFSoftDeleteRepository<InventoryManagerDataContext, Product>
    {
        public ProductRepository(InventoryManagerDataContext dataContext) : base(dataContext) { }
    }
}
