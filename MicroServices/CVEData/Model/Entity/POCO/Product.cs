using System.Collections.Generic;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public class Product : SoftDeletedEntity
    {
        public virtual Vendor Vendor { get; set; }
        public string Name { get; set; }
        public virtual List<CPE> CPEs { get; set; }
    }
}
