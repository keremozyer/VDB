using System;
using System.ComponentModel;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.AssignProductVersionToServer
{
    [DisplayName("Sunucuya Ürün Versyionu Atama İsteği")]
    public record AssignProductVersionToServerRequestModel
    {
        /// <summary>
        /// Servers id.
        /// </summary>
        [DisplayName("Sunucu IDsi")]
        public Guid ServerID { get; set; }
        /// <summary>
        /// ProductVersions id.
        /// </summary>
        [DisplayName("Versiyon IDsi")]
        public Guid ProductVersionID { get; set; }
    }
}
