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
    
    public partial class tblItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblItem()
        {
            this.tblOrderItems = new HashSet<tblOrderItem>();
        }
    
        public System.Guid ItemGuid { get; set; }
        public string ItemName { get; set; }
        public System.Guid RestaurantGuid { get; set; }
        public Nullable<System.Guid> CategoryGuid { get; set; }
        public Nullable<decimal> DeliveryCharge { get; set; }
        public string ItemImage { get; set; }
        public string ItemWeight { get; set; }
        public string ItemSize { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Discount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid CreatedByGuid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.Guid> DeletedByGuid { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> Modifiedby { get; set; }
    
        public virtual tblLookUpDomainValue tblLookUpDomainValue { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderItem> tblOrderItems { get; set; }
    }
}
