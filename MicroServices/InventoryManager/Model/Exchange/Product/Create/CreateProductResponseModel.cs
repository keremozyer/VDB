using System;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create
{
    public record CreateProductResponseModel
    {
        /// <summary>
        /// ID of created product
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Name of created product. May be processed and converted before storing so it may not be same with the name parameter in the request.
        /// </summary>
        public string Name { get; set; }
    }
}
