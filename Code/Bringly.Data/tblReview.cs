//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bringly.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblReview
    {
        public long ReviewId { get; set; }
        public System.Guid ReviewGuid { get; set; }
        public System.Guid FK_BusinessGuid { get; set; }
        public System.Guid FK_UserGuid { get; set; }
        public byte Rating { get; set; }
        public string Review { get; set; }
        public System.Guid FK_OrderGuid { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<System.Guid> ApprovedByGuid { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid FK_CreatedByGuid { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> ModifiedByGuid { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.Guid> FK_DeletedByGuid { get; set; }
        public Nullable<bool> IsProcessed { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsReviewed { get; set; }
        public Nullable<System.Guid> FK_BranchGuid { get; set; }
    
        public virtual tblBranch tblBranch { get; set; }
        public virtual tblBusiness tblBusiness { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
        public virtual tblUser tblUser3 { get; set; }
        public virtual tblOrder tblOrder { get; set; }
    }
}
