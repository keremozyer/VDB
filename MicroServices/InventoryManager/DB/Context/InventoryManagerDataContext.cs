using Microsoft.EntityFrameworkCore;
using VDB.Architecture.Data.Context;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.DB.Context
{
    public class InventoryManagerDataContext : BaseEFDataContext
    {
        public InventoryManagerDataContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}
