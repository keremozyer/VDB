using System;

namespace VDB.MicroServices.CVEData.Model.Exchange.CVEDownload.SearchAndDownload
{
    public record SearchAndDownloadCVERequestModel
    {
        public DateTime SearchDate { get; set; } 
        public ushort ResultsPerPage { get; set; }
        public uint StartIndex { get; set; }
    }
}
