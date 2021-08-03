using System;
using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create
{
    [DisplayName("Ürün Oluşturma İsteği")]
    public record CreateProductRequestModel
    {
        [DisplayName("Firma ID")]
        public Guid VendorID { get; set; }
        [DisplayName("Ürün Adı")]
        public string Name { get; set; }
    }
}
