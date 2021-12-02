namespace VDB.MicroServices.CVEData.Concern.Options
{
    public class CVEDownloaderSettings
    {
        public double DownloadFrequency { get; set; }
        public string DefaultDownloadStartingDateValue { get; set; }
        public string DefaultDownloadStartingDateFormat { get; set; }
        public ushort CVEResultsPerPage { get; set; }
        public bool TriggerReportCreation { get; set; }
    }
}
