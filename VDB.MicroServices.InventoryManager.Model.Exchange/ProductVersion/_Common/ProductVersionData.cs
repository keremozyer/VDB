using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common
{
    public record ProductVersionData
    {
        public Guid ID { get; set; }
        public string Version { get; set; }
        public ProductData Product { get; set; }
        public IEnumerable<ServerData> Servers { get; set; }
    }
}
