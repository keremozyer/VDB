using System.Collections.Generic;
using System.ComponentModel;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.InventoryManager.Model.Entity.POCO
{
    [DisplayName("Firma")]
    public class Vendor : SoftDeletedEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
