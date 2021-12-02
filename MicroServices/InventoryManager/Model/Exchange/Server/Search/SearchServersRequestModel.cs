using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search
{
    [DisplayName("Sunucu Arama İsteği")]
    public record SearchServersRequestModel
    {
        /// <summary>
        /// Server name to be searched. Search is performed in a case invariant manner. If is null or whitespace all servers will be fetched.
        /// </summary>
        [DisplayName("Sunucu Adı")]
        public string Name { get; set; }
        /// <summary>
        /// If true includes products contained in this server.
        /// </summary>
        [DisplayName("İçerdiği Ürünleri Dahil Et")]
        public bool IncludeProducts { get; set; }
    }
}
