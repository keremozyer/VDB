using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common
{
    public record ServerData
    {
        /// <summary>
        /// Servers id.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Servers name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product versions contained in this server.
        /// </summary>
        public IEnumerable<ProductVersionData> ProductVersions { get; set; }
    }
}
