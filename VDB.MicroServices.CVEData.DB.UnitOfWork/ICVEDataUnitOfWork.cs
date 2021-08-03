using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.UnitOfWork
{
    public interface ICVEDataUnitOfWork : IBaseUnitOfWork
    {
        void ClearChangeTrakcer();
        int GetTrackedEntityCount();
        IRepository<CPE> CPERepository { get; }
        IRepository<CVE> CVERepository { get; }
        IRepository<CVEDownloadLog> CVEDownloadLogRepository { get; }
        IRepository<CVENode> CVENodeRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Vendor> VendorRepository { get; }
    }
}
