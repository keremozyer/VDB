using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.InventoryManager.Model.Entity.POCO
{
    [DisplayName("Ürün Versiyonu")]
    public class ProductVersion : SoftDeletedEntity
    {
        public string Version { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        public virtual List<Server> Servers { get; set; }
    }
}
