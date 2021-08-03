using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create
{
    [DisplayName("Sunucu Oluşturma İsteği")]
    public record CreateServerRequestModel
    {
        [DisplayName("Sunucu Adı")]
        public string Name { get; set; }
    }
}
