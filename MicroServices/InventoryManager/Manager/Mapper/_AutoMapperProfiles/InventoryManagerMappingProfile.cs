using AutoMapper;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create;

namespace VDB.MicroServices.InventoryManager.Manager.Mapper._AutoMapperProfiles
{
    public class InventoryManagerMappingProfile : Profile
    {
        public InventoryManagerMappingProfile()
        {
            CreateMap<CreateVendorRequestModel, Vendor>();
            CreateMap<Vendor, CreateVendorResponseModel>();

            CreateMap<CreateProductRequestModel, Product>();
            CreateMap<Product, CreateProductResponseModel>();

            CreateMap<CreateServerRequestModel, Server>();
            CreateMap<Server, CreateServerResponseModel>();

            CreateMap<CreateProductVersionRequestModel, ProductVersion>();
            CreateMap<ProductVersion, CreateProductVersionResponseModel>();

            CreateMap<Server, ServerData>();
            CreateMap<ProductVersion, ProductVersionData>();
            CreateMap<Product, ProductData>();
            CreateMap<Vendor, VendorData>();
        }
    }
}
