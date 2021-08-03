using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.GetVersionsOfProduct
{
    public record GetVersionsOfProductResponseModel
    {
        public List<ProductVersionData> ProductVersions { get; set; }
    }
}
