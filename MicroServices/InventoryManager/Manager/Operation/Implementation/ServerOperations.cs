using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Implementation
{
    public class ServerOperations : IServerOperations
    {
        private readonly IInventoryManagerUnitOfWork DB;

        public ServerOperations(IInventoryManagerUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(Server server)
        {
            this.DB.ServerRepository.Create(server);
        }

        public void Delete(Server server)
        {
            this.DB.ServerRepository.Delete(server);
        }

        public void Update(Server server)
        {
            this.DB.ServerRepository.Update(server);
        }

        public async Task<Server> GetServerAsync(Guid id, params Expression<Func<Server, object>>[] includes)
        {
            return await this.DB.ServerRepository.GetFirstAsync(s => s.Id == id, includes: includes);
        }

        public async Task<Server> GetServerByNameAsync(string name)
        {
            return await this.DB.ServerRepository.GetFirstAsync(s => s.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Searches db for given name. Makes case invariant comparison.
        /// </summary>
        /// <param name="name">Server name</param>
        /// <param name="includes">Other entities to include in search.</param>
        /// <returns>List of servers.</returns>
        public async Task<List<Server>> SearchByName(string name, params Expression<Func<Server, object>>[] includes)
        {
            return await this.DB.ServerRepository.GetAsync(s => s.Name.ToLower() == name.ToLower(), includes: includes);
        }

        public async Task<List<Server>> GetAllServers(params Expression<Func<Server, object>>[] includes)
        {
            return await this.DB.ServerRepository.GetAsync(filter: null, includes: includes);
        }
    }
}
