using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common
{
    public record VendorData
    {
        /// <summary>
        /// Vendors id.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Vendors name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Products of this vendor.
        /// </summary>
        public IEnumerable<ProductData> Products { get; set; }
    }
}
