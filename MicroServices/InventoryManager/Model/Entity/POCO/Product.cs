using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.InventoryManager.Model.Entity.POCO
{
    [DisplayName("Ürün")]
    public class Product : SoftDeletedEntity
    {
        public string Name { get; set; }
        public virtual List<ProductVersion> ProductVersions { get; set; }
        public Guid VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual Vendor Vendor { get; set; }
    }
}
