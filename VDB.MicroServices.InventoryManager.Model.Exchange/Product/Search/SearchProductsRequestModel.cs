using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search
{
    [DisplayName("Ürün Arama İsteği")]
    public record SearchProductsRequestModel
    {
        [DisplayName("Ürün Adı")]
        public string Name { get; set; }
        [DisplayName("Versiyonları Dahil Et")]
        public bool IncludeVersions { get; set; }
        [DisplayName("Firma Bilgisini Dahil Et")]
        public bool IncludeVendorData { get; set; }
        [DisplayName("Bulunduğu Sunucuları Dahil Et")]
        public bool IncludeContainingServers { get; set; }
    }
}
