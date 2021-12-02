using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.AssignProductVersionToServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.GetProductsOnServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Interface
{
    public interface IServerBusinessManager
    {
        public Task<CreateServerResponseModel> CreateServer(CreateServerRequestModel request);
        public Task DeleteServer(Guid id);
        public Task AssignProductVersionToServer(AssignProductVersionToServerRequestModel request);
        public Task<SearchServersResponseModel> Search(SearchServersRequestModel request);
        Task<GetProductVersionsOnServerResponseModel> GetProductsOnServer(Guid serverID);
        Task RemoveProductVersionFromServer(Guid serverID, Guid productVersionID);
    }
}
