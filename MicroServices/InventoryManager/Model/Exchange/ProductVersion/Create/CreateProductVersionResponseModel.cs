namespace VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create
{
    public record CreateProductVersionResponseModel
    {
        /// <summary>
        /// Product versions ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Version number of created product version. May be processed and converted before storing so it may not be same with the version parameter in the request.
        /// </summary>
        public string Version { get; set; }
    }
}
