using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Interface
{
    public interface IServerOperations
    {
        public void Create(Server server);
        public void Delete(Server server);
        public void Update(Server server);
        public Task<Server> GetServerAsync(Guid id, params Expression<Func<Server, object>>[] includes);
        public Task<Server> GetServerByNameAsync(string name);
        public Task<List<Server>> SearchByName(string name, params Expression<Func<Server, object>>[] includes);
        public Task<List<Server>> GetAllServers(params Expression<Func<Server, object>>[] includes);
    }
}
