using System.Net.Http;
using System.IO;
using VDB.Architecture.Concern.Helper;
using System.Threading.Tasks;
using System.Linq;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.CVEData.ExternalData.Model.CVEData.NVD;
using VDB.MicroServices.CVEData.Model.DTO.CVEData;
using VDB.MicroServices.CVEData.Manager.Mapper.ExternalCVEDataMappers;
using System;
using Microsoft.Extensions.Options;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.ExternalData.Manager.Contract;

namespace VDB.MicroServices.CVEData.ExternalData.Manager.Service.CVEData.NVD
{
    public class NVDServiceManager : ICVEServiceManager
    {
        private readonly NVDSettings NVDSettings;
        private readonly HttpClient HttpClient;
        private readonly NVDSecrets NVDSecrets;

        public NVDServiceManager(IOptions<EndpointSettings> endpointSettings, HttpClient httpClient, IOptions<Secrets> secrets)
        {
            this.NVDSettings = endpointSettings.Value.NVD;
            this.HttpClient = httpClient;
            this.NVDSecrets = secrets.Value.NVD;
        }

        public async Task<CVEResult> DownloadYearlyData(int year)
        {
            HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(String.Format(this.NVDSettings.DownloadYearlyDataEndpoint, year));

            Stream fileStream = CompressionHelpers.OpenCompressedFileStreamAsStreams(await httpResponse.Content.ReadAsStreamAsync()).First();
            YearlyCVEResponse nvdResponse = (await fileStream.ReadAsStringAsync()).DeserializeJSON<YearlyCVEResponse>();

            return NVDMapper.MapYearlyResult(nvdResponse);
        }

        public async Task<CVEResult> Search(DateTime searchDate, ushort resultsPerPage, uint startIndex)
        {
            HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(String.Format(this.NVDSettings.SearchEndpoint, resultsPerPage, searchDate.ToUniversalTime().ToString(this.NVDSettings.DateTimeFormat), startIndex, DateTime.UtcNow.ToString(this.NVDSettings.DateTimeFormat), NVDSecrets.APIKey));

            CVESearchResponse nvdResponse = (await httpResponse.Content.ReadAsStringAsync()).DeserializeJSON<CVESearchResponse>();

            return NVDMapper.MapSearchResult(nvdResponse);
        }
    }
}
