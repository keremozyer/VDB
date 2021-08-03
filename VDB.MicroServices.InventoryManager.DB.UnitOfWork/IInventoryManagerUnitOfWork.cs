using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.UnitOfWork
{
    public interface IInventoryManagerUnitOfWork: IBaseUnitOfWork
    {        
        IRepository<Product> ProductRepository { get; }
        IRepository<ProductVersion> ProductVersionRepository { get; }
        IRepository<Server> ServerRepository { get; }
        IRepository<Vendor> VendorRepository{ get; }
    }
}
