using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Exchange.CVESearch.SearchByProductVersion;

namespace VDB.MicroServices.CVEData.Manager.Business.Interface
{
    public interface ICVESearchBusinessManager
    {
        Task<SearchByProductVersionResponseModel> SearchByProductVersion(SearchByProductVersionRequestModel request);
    }
}
