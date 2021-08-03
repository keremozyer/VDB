using System;
using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create
{
    [DisplayName("Ürün Versiyonu Oluşturma İsteği")]
    public record CreateProductVersionRequestModel
    {
        [DisplayName("Ürün IDsi")]
        public Guid ProductID { get; set; }
        [DisplayName("Versiyon")]
        public string Version { get; set; }
    }
}
