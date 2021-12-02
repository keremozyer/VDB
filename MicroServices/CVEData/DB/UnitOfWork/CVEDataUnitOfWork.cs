using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.DB.Repository.EntityFramework;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.UnitOfWork
{
    public class CVEDataUnitOfWork : BaseUnitOfWork, ICVEDataUnitOfWork
    {
        public CVEDataUnitOfWork(CVEDataDataContext context) : base(context) { }

        public int GetTrackedEntityCount()
        {
            return context.GetTrackedEntityCount();
        }

        private CPERepository cpeRepository { get; set; }
        public IRepository<CPE> CPERepository { get { if (cpeRepository == null) { cpeRepository = new CPERepository((CVEDataDataContext)context); } return cpeRepository; } }

        private CVERepository cveRepository { get; set; }
        public IRepository<CVE> CVERepository { get { if (cveRepository == null) { cveRepository = new CVERepository((CVEDataDataContext)context); } return cveRepository; } }

        private CVEDownloadLogRepository cveDownloadLogRepository { get; set; }
        public IRepository<CVEDownloadLog> CVEDownloadLogRepository { get { if (cveDownloadLogRepository == null) { cveDownloadLogRepository = new CVEDownloadLogRepository((CVEDataDataContext)context); } return cveDownloadLogRepository; } }

        private CVENodeRepository cveNodeRepository { get; set; }
        public IRepository<CVENode> CVENodeRepository { get { if (cveNodeRepository == null) { cveNodeRepository = new CVENodeRepository((CVEDataDataContext)context); } return cveNodeRepository; } }

        private ProductRepository productRepository { get; set; }
        public IRepository<Product> ProductRepository { get { if (productRepository == null) { productRepository = new ProductRepository((CVEDataDataContext)context); } return productRepository; } }

        private VendorRepository vendorRepository { get; set; }
        public IRepository<Vendor> VendorRepository { get { if (vendorRepository == null) { vendorRepository = new VendorRepository((CVEDataDataContext)context); } return vendorRepository; } }
    }
}
