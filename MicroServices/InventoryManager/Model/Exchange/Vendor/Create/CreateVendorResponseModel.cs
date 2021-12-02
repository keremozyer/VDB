namespace VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create
{
    public record CreateVendorResponseModel
    {
        /// <summary>
        /// ID of created vendor.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Name of created vendor. May be processed and converted before storing so it may not be same with the name parameter in the request.
        /// </summary>
        public string Name { get; set; }
    }
}
