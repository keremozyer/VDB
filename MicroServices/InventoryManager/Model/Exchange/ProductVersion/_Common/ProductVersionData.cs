using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common
{
    public record ProductVersionData
    {
        /// <summary>
        /// Product Versions id.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Version number.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Product of this Product Version
        /// </summary>
        public ProductData Product { get; set; }
        /// <summary>
        /// Servers containing this product version.
        /// </summary>
        public IEnumerable<ServerData> Servers { get; set; }
    }
}
