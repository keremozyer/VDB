using Microsoft.EntityFrameworkCore;
using VDB.Architecture.Data.Context;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.Context
{
    public class CVEDataDataContext : BaseEFDataContext
    {
        public CVEDataDataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CVENode>().HasOne(cn => cn.ParentNode).WithMany(cn => cn.ChildrenNodes).HasForeignKey(cn => cn.ParentNodeId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }

        public DbSet<CPE> CPEs { get; set; }
        public DbSet<CVE> CVEs { get; set; }
        public DbSet<CVEDownloadLog> CVEDownloadLogs { get; set; }
        public DbSet<CVENode> CVENodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}
