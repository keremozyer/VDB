using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.GetVersionsOfProduct;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Interface
{
    public interface IProductVersionBusinessManager
    {
        public Task<CreateProductVersionResponseModel> CreateProductVersion(CreateProductVersionRequestModel request);        
        Task<GetVersionsOfProductResponseModel> GetVersionsOfProduct(Guid productID);        
        public Task DeleteProductVersion(Guid id);
        void DeleteProductVersion(ProductVersion version);
    }
}
