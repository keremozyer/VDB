using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.CVEData.Model.Entity.POCO
{
    public class CVENode : SoftDeletedEntity
    {
        public virtual CVE CVE { get; set; }
        public string Operator { get; set; }
        public Guid? ParentNodeId { get; set; }
        [ForeignKey(nameof(ParentNodeId))]
        public virtual CVENode ParentNode { get; set; }
        public virtual List<CVENode> ChildrenNodes { get; set; }
        public virtual List<CPE> CPEs { get; set; }
    }
}
