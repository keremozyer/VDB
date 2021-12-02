using System;
using System.Collections.Generic;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public class CVE : SoftDeletedEntity
    {
        public string CVEID { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Description { get; set; }
        public virtual List<CVENode> Nodes { get; set; }
    }
}
