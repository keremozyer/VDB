using System.Collections.Generic;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;

namespace VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search
{
    public record SearchServersResponseModel
    {
        /// <summary>
        /// List of servers.
        /// </summary>
        public IEnumerable<ServerData> Servers { get; set; }

        public SearchServersResponseModel(IEnumerable<ServerData> servers)
        {
            this.Servers = servers;
        }        
    }
}
