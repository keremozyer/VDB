namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create
{
    public record CreateServerResponseModel
    {
        /// <summary>
        /// ID of created server.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Name of created server. May be processed and converted before storing so it may not be same with the name parameter in the request.
        /// </summary>
        public string Name { get; set; }
    }
}
