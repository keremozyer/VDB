using System;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public class CVEDownloadLog : HardDeletedEntity
    {
        public DateTime CVEDataTimestamp { get; set; }
        public bool IsDownloadBySearch { get; set; }
    }
}
