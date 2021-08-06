using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search
{
    [DisplayName("Ürün Arama İsteği")]
    public record SearchProductsRequestModel
    {
        /// <summary>
        /// Product name to be searched. Search is performed in a case invariant manner. If is null or whitespace all products will be fetched.
        /// </summary>
        [DisplayName("Ürün Adı")]
        public string Name { get; set; }
        /// <summary>
        /// If true includes products versions in response.
        /// </summary>
        [DisplayName("Versiyonları Dahil Et")]
        public bool IncludeVersions { get; set; }
        /// <summary>
        /// If true includes products vendor data in response.
        /// </summary>
        [DisplayName("Firma Bilgisini Dahil Et")]
        public bool IncludeVendorData { get; set; }
        /// <summary>
        /// If true includes servers containing this product in response.
        /// </summary>
        [DisplayName("Bulunduğu Sunucuları Dahil Et")]
        public bool IncludeContainingServers { get; set; }
    }
}
