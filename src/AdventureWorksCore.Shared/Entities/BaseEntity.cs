using System;

namespace AdventureWorksCore.Shared
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}