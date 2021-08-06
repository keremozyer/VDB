using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common
{
    public record ProductData
    {
        /// <summary>
        /// Products id.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Products name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Versions of this product.
        /// </summary>
        public IEnumerable<ProductVersionData> ProductVersions { get; set; }
        /// <summary>
        /// Vendor of this product.
        /// </summary>
        public VendorData Vendor { get; set; }
    }
}
