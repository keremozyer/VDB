using System;
using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common
{
    public record ServerData
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductVersionData> ProductVersions { get; set; }
    }
}
