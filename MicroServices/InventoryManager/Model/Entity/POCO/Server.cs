using System.Collections.Generic;
using System.ComponentModel;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.InventoryManager.Model.Entity.POCO
{
    [DisplayName("Sunucu")]
    public class Server : SoftDeletedEntity
    {
        public string Name { get; set; }
        public virtual List<ProductVersion> ProductVersions { get; set; }
    }
}
