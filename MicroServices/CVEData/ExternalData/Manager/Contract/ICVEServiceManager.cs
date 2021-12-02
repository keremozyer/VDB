using System;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.DTO.CVEData;

namespace VDB.MicroServices.CVEData.ExternalData.Manager.Contract
{
    public interface ICVEServiceManager
    {
        Task<CVEResult> DownloadYearlyData(int year);
        Task<CVEResult> Search(DateTime searchDate, ushort resultsPerPage, uint startIndex);
    }
}
