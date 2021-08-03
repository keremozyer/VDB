using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common
{
    public record ProductData
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductVersionData> ProductVersions { get; set; }
        public VendorData Vendor { get; set; }
    }
}
