using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model.Derived.DataNotFound;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.GenericValidator;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.AssignProductVersionToServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.GetProductsOnServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Implementation
{
    public class ServerBusinessManager : IServerBusinessManager
    {
        private readonly Validator Validator;
        private readonly IMapper Mapper;
        private readonly IServerOperations ServerOperations;
        private readonly IProductVersionOperations ProductVersionOperations;

        public ServerBusinessManager(Validator validator, IMapper mapper, IServerOperations serverOperations, IProductVersionOperations productVersionOperations)
        {
            this.Validator = validator;
            this.Mapper = mapper;
            this.ServerOperations = serverOperations;
            this.ProductVersionOperations = productVersionOperations;
        }

        /// <summary>
        /// Searches db for given server name (with lowercase comparasion) and creates a new server if given name is not found.
        /// </summary>
        /// <param name="request">Server data.</param>
        /// <returns>Data of found or created server.</returns>
        public async Task<CreateServerResponseModel> CreateServer(CreateServerRequestModel request)
        {
            this.Validator.Validate<CreateServerRequestModel>(request);

            Server server = await this.ServerOperations.GetServerByNameAsync(request.Name);
            if (server == null)
            {
                server = this.Mapper.Map<Server>(request);
                server.Name = server.Name.ToLower();
                this.ServerOperations.Create(server);
            }

            return this.Mapper.Map<CreateServerResponseModel>(server);
        }

        /// <summary>
        /// Deletes server with given id. If server does not exists in db throws an exception.
        /// </summary>
        /// <param name="id">Server to delete</param>
        /// <returns></returns>
        public async Task DeleteServer(Guid id)
        {
            Server server = await this.ServerOperations.GetServerAsync(id);
            if (server == null) throw new DataNotFoundException(entityName: typeof(Server).GetDisplayName(), value: id);

            this.ServerOperations.Delete(server);
        }

        /// <summary>
        /// Assigns given productVersion to given server if it's not already assigned.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AssignProductVersionToServer(AssignProductVersionToServerRequestModel request)
        {
            this.Validator.Validate<AssignProductVersionToServerRequestModel>(request);

            Server server = await this.ServerOperations.GetServerAsync(request.ServerID, i => i.ProductVersions);
            if (server == null) throw new DataNotFoundException(entityName: typeof(Server).GetDisplayName(), value: request.ServerID);

            ProductVersion productVersion = await this.ProductVersionOperations.GetProductVersionAsync(request.ProductVersionID);
            if (productVersion == null) throw new DataNotFoundException(entityName: typeof(ProductVersion).GetDisplayName(), value: request.ProductVersionID);

            server.ProductVersions ??= new List<ProductVersion>();
            if (!server.ProductVersions.Any(pv => pv.Id == request.ProductVersionID))
            {
                server.ProductVersions.Add(productVersion);
                this.ServerOperations.Update(server);
            }
        }

        /// <summary>
        /// Searches for servers starting with given name. Makes case invariant comparison.
        /// </summary>
        /// <param name="request">Search data.</param>
        /// <returns>List of servers.</returns>
        public async Task<SearchServersResponseModel> Search(SearchServersRequestModel request)
        {
            Expression<Func<Server, object>>[] includes = null;
            if (request.IncludeProducts)
            {
                includes = new Expression<Func<Server, object>>[] { i => i.ProductVersions.Select(pv => pv.Product).Select(p => p.Vendor) };
            }
            
            List<Server> servers = String.IsNullOrWhiteSpace(request.Name) ? await this.ServerOperations.GetAllServers(includes?.ToArray()) : await this.ServerOperations.SearchByName(request.Name, includes: includes);

            return new SearchServersResponseModel(servers?.Select(server => this.Mapper.Map<ServerData>(server))?.OrderBy(s => s.Name));
        }

        public async Task<GetProductVersionsOnServerResponseModel> GetProductsOnServer(Guid serverID)
        {
            Server server = await this.ServerOperations.GetServerAsync(serverID, i => i.ProductVersions.Select(p => p.Product.Vendor));
            if (server == null) throw new DataNotFoundException(entityName: typeof(Server).GetDisplayName(), value: serverID);

            return new GetProductVersionsOnServerResponseModel() { ProductVersions = server.ProductVersions?.Select(pv => this.Mapper.Map<ProductVersionData>(pv))?.ToList() };
        }

        public async Task RemoveProductVersionFromServer(Guid serverID, Guid productVersionID)
        {
            Server server = await this.ServerOperations.GetServerAsync(serverID, i => i.ProductVersions);
            if (server == null) throw new DataNotFoundException(entityName: typeof(Server).GetDisplayName(), value: serverID);

            ProductVersion productVersionToRemove = server.ProductVersions?.FirstOrDefault(pv => pv.Id == productVersionID);
            if (productVersionToRemove != null)
            {
                server.ProductVersions.Remove(productVersionToRemove);
                this.ServerOperations.Update(server);
            }
        }
    }
}
