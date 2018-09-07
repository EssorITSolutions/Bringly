using System;

namespace Bringly.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Manager
    {
        public System.Guid ManagerGuid { get; set; }
        public Nullable<System.Guid> BusinessGuid { get; set; }
        public Nullable<System.Guid> BranchGuid { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public System.Guid CreatedByGuid { get; set; }
        public string Name { get; set; }
    }
}
