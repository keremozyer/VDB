using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search
{
    public class SearchVendorsResponseModel
    {
        public IEnumerable<VendorData> Vendors { get; set; }

        public SearchVendorsResponseModel(IEnumerable<VendorData> vendors)
        {
            this.Vendors = vendors;
        }
    }
}
