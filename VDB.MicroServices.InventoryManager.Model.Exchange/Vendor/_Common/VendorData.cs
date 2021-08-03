using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common
{
    public record VendorData
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductData> Products { get; set; }
    }
}
