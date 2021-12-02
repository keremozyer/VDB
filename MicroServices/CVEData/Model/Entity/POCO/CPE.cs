using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public partial class CPE : SoftDeletedEntity
    {
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        public virtual List<CVENode> CVENodes { get; set; }
        public string URI { get; set; }
        public bool IsVulnerable { get; set; }
        public string SpecificVersion { get; set; }
        public string VersionStartIncluding { get; set; }
        public string VersionEndIncluding { get; set; }
        public string VersionStartExcluding { get; set; }
        public string VersionEndExcluding { get; set; }
    }

    public partial class CPE
    {
        public string VersionRangeStart { get { return this.VersionStartIncluding ?? this.VersionStartExcluding; } }
        public bool IsStartingVersionInclusive { get { return !String.IsNullOrWhiteSpace(this.VersionStartIncluding); } }
        public string VersionRangeEnd { get { return this.VersionEndIncluding ?? this.VersionEndExcluding; } }
        public bool IsEndingVersionInclusive { get { return !String.IsNullOrWhiteSpace(this.VersionEndIncluding); } }

        public bool HasNonNumericVersionRange(string standartVersionSeperator)
        {
            return
                (!String.IsNullOrWhiteSpace(this.VersionStartIncluding) && !this.VersionStartIncluding.IsAllDigit(standartVersionSeperator)) ||
                (!String.IsNullOrWhiteSpace(this.VersionEndIncluding) && !this.VersionEndIncluding.IsAllDigit(standartVersionSeperator)) ||
                (!String.IsNullOrWhiteSpace(this.VersionStartExcluding) && !this.VersionStartExcluding.IsAllDigit(standartVersionSeperator)) ||
                (!String.IsNullOrWhiteSpace(this.VersionEndExcluding) && !this.VersionEndExcluding.IsAllDigit(standartVersionSeperator));
        }

        public bool HasNumericVersionRange(string standartVersionSeperator)
        {
            return 
                this.VersionStartIncluding.IsAllDigit(standartVersionSeperator) ||
                this.VersionEndIncluding.IsAllDigit(standartVersionSeperator) ||
                this.VersionStartExcluding.IsAllDigit(standartVersionSeperator) ||
                this.VersionEndExcluding.IsAllDigit(standartVersionSeperator);
        }
    }
}
