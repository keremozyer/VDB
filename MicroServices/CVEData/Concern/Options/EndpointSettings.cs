namespace VDB.MicroServices.CVEData.Concern.Options
{
    public class EndpointSettings
    {
        public NVDSettings NVD { get; set; }
        public VulnerabilityDetectorSettings VulnerabilityDetector { get; set; }
    }

    public class NVDSettings
    {
        public string DownloadYearlyDataEndpoint { get; set; }
        public string SearchEndpoint { get; set; }
        public string DateTimeFormat { get; set; }
    }

    public class VulnerabilityDetectorSettings
    {
        public string CreateReportEndpoint { get; set; }
    }
}
