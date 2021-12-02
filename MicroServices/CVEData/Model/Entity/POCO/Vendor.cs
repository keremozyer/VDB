using System.Collections.Generic;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public class Vendor : SoftDeletedEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
