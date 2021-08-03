using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Interface
{
    public interface IVendorBusinessManager
    {
        public Task<CreateVendorResponseModel> CreateVendor(CreateVendorRequestModel request);
        public Task DeleteVendor(Guid id);
        public Task<SearchVendorsResponseModel> Search(SearchVendorsRequestModel request);
    }
}
