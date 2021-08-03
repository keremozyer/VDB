using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.DB.Repository.EntityFramework;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.UnitOfWork
{
    public class InventoryManagerUnitOfWork : BaseUnitOfWork, IInventoryManagerUnitOfWork
    {
        public InventoryManagerUnitOfWork(InventoryManagerDataContext context) : base(context) { }

        private ProductRepository productRepository { get; set; }
        public IRepository<Product> ProductRepository { get { if (productRepository == null) { productRepository = new ProductRepository((InventoryManagerDataContext)context); } return productRepository; } }

        private ProductVersionRepository productVersionRepository { get; set; }
        public IRepository<ProductVersion> ProductVersionRepository { get { if (productVersionRepository == null) { productVersionRepository = new ProductVersionRepository((InventoryManagerDataContext)context); } return productVersionRepository; } }

        private ServerRepository serverRepository { get; set; }
        public IRepository<Server> ServerRepository { get { if (serverRepository == null) { serverRepository = new ServerRepository((InventoryManagerDataContext)context); } return serverRepository; } }

        private VendorRepository vendorRepository { get; set; }
        public IRepository<Vendor> VendorRepository { get { if (vendorRepository == null) { vendorRepository = new VendorRepository((InventoryManagerDataContext)context); } return vendorRepository; } }
    }
}
