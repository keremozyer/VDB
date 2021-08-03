using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search
{
    [DisplayName("Sunucu Arama İsteği")]
    public record SearchServersRequestModel
    {
        [DisplayName("Sunucu Adı")]
        public string Name { get; set; }
        [DisplayName("İçerdiği Ürünleri Dahil Et")]
        public bool IncludeProducts { get; set; }
    }
}
