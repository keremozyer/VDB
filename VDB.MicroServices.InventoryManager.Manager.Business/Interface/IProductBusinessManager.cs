using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Interface
{
    public interface IProductBusinessManager
    {
        public Task<CreateProductResponseModel> CreateProduct(CreateProductRequestModel request);
        public Task DeleteProduct(Guid id);
        public void DeleteProduct(Product product); 
        public Task<SearchProductsResponseModel> Search(SearchProductsRequestModel request);
        public Task<SearchProductsResponseModel> GetProductsOfVendor(Guid vendorID);        
    }
}
