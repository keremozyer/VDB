using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.GetProductsOnServer
{
    public record GetProductVersionsOnServerResponseModel
    {
        /// <summary>
        /// List of product versions
        /// </summary>
        public List<ProductVersionData> ProductVersions { get; set; }
    }
}
