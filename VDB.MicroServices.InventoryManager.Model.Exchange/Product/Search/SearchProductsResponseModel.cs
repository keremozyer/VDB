using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search
{
    public record SearchProductsResponseModel
    {
        public IEnumerable<ProductData> Products { get; set; }

        public SearchProductsResponseModel(IEnumerable<ProductData> products)
        {
            this.Products = products;
        }
    }
}
