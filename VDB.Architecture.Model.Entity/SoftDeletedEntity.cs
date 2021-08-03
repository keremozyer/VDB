using System;

namespace VDB.Architecture.Model.Entity
{
    public abstract class SoftDeletedEntity : BaseEntity
    {
        public DateTime? DeletedAt { get; set; }
    }
}
