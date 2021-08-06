using System;
using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create
{
    [DisplayName("Ürün Versiyonu Oluşturma İsteği")]
    public record CreateProductVersionRequestModel
    {
        /// <summary>
        /// Products id.
        /// </summary>
        [DisplayName("Ürün IDsi")]
        public Guid ProductID { get; set; }
        /// <summary>
        /// Version number.
        /// </summary>
        [DisplayName("Versiyon")]
        public string Version { get; set; }
    }
}
