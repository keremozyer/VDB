using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create
{
    [DisplayName("Firma Oluşturma İsteği")]
    public record CreateVendorRequestModel
    {
        /// <summary>
        /// Vendors name.
        /// </summary>
        [DisplayName("Firma Adı")]
        public string Name { get; set; }
    }
}
