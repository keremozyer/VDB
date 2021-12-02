using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.Repository.EntityFramework
{
    public class VendorRepository : EFSoftDeleteRepository<InventoryManagerDataContext, Vendor>
    {
        public VendorRepository(InventoryManagerDataContext dataContext) : base(dataContext) { }
    }
}
