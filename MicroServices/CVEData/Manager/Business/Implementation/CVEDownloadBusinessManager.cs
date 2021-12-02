using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.ExternalData.Manager.Contract;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.CVEData;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.VulnerabilityReport;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.DTO.CVEData;
using VDB.MicroServices.CVEData.Model.Entity.POCO;
using VDB.MicroServices.CVEData.Model.Exchange.CVEDownload.SearchAndDownload;

namespace VDB.MicroServices.CVEData.Manager.Business.Implementation
{
    public class CVEDownloadBusinessManager : ICVEDownloadBusinessManager
    {
        private readonly ICVEDataUnitOfWork DB;
        private readonly ICVEServiceManager CVEServiceManager;
        private readonly ICVEDataStorageBusinessManager CVEDataStorageManager;
        private readonly ICVEDownloadLogOperations CVEDownloadLogOperations;
        private readonly CVEDownloaderSettings CVEDownloaderSettings;
        private readonly IVulnerabilityReportServiceManager VulnerabilityReportServiceManager;

        public CVEDownloadBusinessManager(ICVEDataUnitOfWork db, ICVEServiceManager cveServiceManager, ICVEDataStorageBusinessManager cveDataStorageManager, ICVEDownloadLogOperations cveDownloadLogOperations, IOptions<CVEDownloaderSettings> cveDownloaderSettings, IVulnerabilityReportServiceManager vulnerabilityReportServiceManager)
        {
            this.DB = db;
            this.CVEServiceManager = cveServiceManager;
            this.CVEDataStorageManager = cveDataStorageManager;
            this.CVEDownloadLogOperations = cveDownloadLogOperations;
            this.CVEDownloaderSettings = cveDownloaderSettings.Value;
            this.VulnerabilityReportServiceManager = vulnerabilityReportServiceManager;
        }

        public async Task GetSystemUpToDate()
        {
            CVEDownloadLog latestDownloadLog = await this.CVEDownloadLogOperations.GetLatestDownloadLog();
            DateTime? latestDownloadTimestamp = latestDownloadLog?.CVEDataTimestamp ?? DateTime.ParseExact(CVEDownloaderSettings.DefaultDownloadStartingDateValue, CVEDownloaderSettings.DefaultDownloadStartingDateFormat, null);

            latestDownloadTimestamp = await CheckAndDownloadYearlyData(latestDownloadLog, latestDownloadTimestamp.Value);

            int newRecordCount = await CheckAndDownloadTillNow(latestDownloadTimestamp.Value);

            if (newRecordCount > 0 && this.CVEDownloaderSettings.TriggerReportCreation)
            {
                await this.VulnerabilityReportServiceManager.CreateReport();
            }
        }

        private async Task<DateTime> CheckAndDownloadYearlyData(CVEDownloadLog latestDownloadLog, DateTime latestDownloadTimestamp)
        {
            DateTime serviceSearchTimestamp = latestDownloadTimestamp;
            if (!(latestDownloadLog?.IsDownloadBySearch).GetValueOrDefault())
            {
                while (latestDownloadTimestamp.Year < DateTime.Now.Year)
                {
                    latestDownloadTimestamp = latestDownloadTimestamp.AddYears(1);
                    serviceSearchTimestamp = await DownloadYearlyData(latestDownloadTimestamp.Year);                    
                    this.CVEDownloadLogOperations.Create(new CVEDownloadLog() { CVEDataTimestamp = latestDownloadTimestamp });
                }

                await this.DB.SaveAsync();
            }

            return serviceSearchTimestamp;
        }

        private async Task<int> CheckAndDownloadTillNow(DateTime latestDownloadTimestamp)
        {
            uint cveSearchStartIndex = 0;
            int totalCVECount;
            DateTime newLatestDownloadTimestamp;
            do
            {
                CVEResult result = await SearchAndDownload(new SearchAndDownloadCVERequestModel() { SearchDate = latestDownloadTimestamp, StartIndex = cveSearchStartIndex, ResultsPerPage = CVEDownloaderSettings.CVEResultsPerPage });
                totalCVECount = result.TotalCVECount;
                newLatestDownloadTimestamp = result.SearchTimestamp;
                cveSearchStartIndex += CVEDownloaderSettings.CVEResultsPerPage;
            } while (cveSearchStartIndex < totalCVECount);

            this.CVEDownloadLogOperations.Create(new CVEDownloadLog() { CVEDataTimestamp = newLatestDownloadTimestamp, IsDownloadBySearch = true });

            await this.DB.SaveAsync();

            return totalCVECount;
        }

        private async Task<DateTime> DownloadYearlyData(int year)
        {
            CVEResult result = await this.CVEServiceManager.DownloadYearlyData(year);

            await this.CVEDataStorageManager.StoreCVEResult(result);

            return result.SearchTimestamp;
        }

        private async Task<CVEResult> SearchAndDownload(SearchAndDownloadCVERequestModel request)
        {
            CVEResult result = await this.CVEServiceManager.Search(request.SearchDate, request.ResultsPerPage, request.StartIndex);
            
            await this.CVEDataStorageManager.StoreCVEResult(result);

            return result;
        }
    }
}
