using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search
{
    [DisplayName("Firma Arama İsteği")]
    public record SearchVendorsRequestModel
    {
        [DisplayName("Firma Adı")]
        public string Name { get; set; }
        [DisplayName("Firmanın Ürünlerini Dahil Et")]
        public bool IncludeProducts { get; set; }
        [DisplayName("Firmayı İçeren Sunucuları Dahil Et")]
        public bool IncludeContainingServers { get; set; }
    }
}
