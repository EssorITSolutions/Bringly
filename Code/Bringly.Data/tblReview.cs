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
        public System.Guid ReviewGuid { get; set; }
        public System.Guid RestaurantGuid { get; set; }
        public System.Guid UserGuid { get; set; }
        public byte Rating { get; set; }
        public string Review { get; set; }
        public System.Guid OrderGuid { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSkipped { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsProcessed { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid CreatedByGuid { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> ModifiedByGuid { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.Guid> DeletedByGuid { get; set; }
    
        public virtual tblRestaurant tblRestaurant { get; set; }
        public virtual tblOrder tblOrder { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
        public virtual tblUser tblUser3 { get; set; }
    }
}
