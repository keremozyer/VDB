using AutoMapper;

namespace VDB.MicroServices.CVEData.Manager.Mapper._AutoMapperProfiles
{
    public class CVEDataMappingProfile : Profile
    {
        public CVEDataMappingProfile()
        {
            CreateMap<Model.Entity.POCO.CVE, Model.Exchange.CVESearch.SearchByProductVersion.CVEData>()
                .ForMember(c => c.AdditionalRequiredProducts, o => o.Ignore())
                .ForMember(c => c.MatchType, o => o.Ignore());
            CreateMap<Model.Entity.POCO.CPE, Model.Exchange.CVESearch.SearchByProductVersion.ProductData>()
                .ForMember(p => p.VendorName, o => o.MapFrom(c => c.Product.Vendor.Name))
                .ForMember(p => p.ProductName, o => o.MapFrom(c => c.Product.Name));
        }
    }
}
