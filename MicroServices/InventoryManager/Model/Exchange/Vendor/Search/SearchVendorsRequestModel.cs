using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search
{
    [DisplayName("Firma Arama İsteği")]
    public record SearchVendorsRequestModel
    {
        /// <summary>
        /// Vendor name to be searched. Search is performed in a case invariant manner. If is null or whitespace all vendors will be fetched.
        /// </summary>
        [DisplayName("Firma Adı")]
        public string Name { get; set; }
        /// <summary>
        /// If true includes products of this vendor.
        /// </summary>
        [DisplayName("Firmanın Ürünlerini Dahil Et")]
        public bool IncludeProducts { get; set; }
        /// <summary>
        /// If true includes servers containing this vendor.
        /// </summary>
        [DisplayName("Firmayı İçeren Sunucuları Dahil Et")]
        public bool IncludeContainingServers { get; set; }
    }
}
