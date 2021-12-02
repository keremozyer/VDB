using System;
using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create
{
    [DisplayName("Ürün Oluşturma İsteği")]
    public record CreateProductRequestModel
    {
        /// <summary>
        /// ID of vendor which owns this product.
        /// </summary>
        [DisplayName("Firma ID")]
        public Guid VendorID { get; set; }
        /// <summary>
        /// Name of product.
        /// </summary>
        [DisplayName("Ürün Adı")]
        public string Name { get; set; }
    }
}
