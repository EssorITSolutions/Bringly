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
    
    public partial class tblEmail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEmail()
        {
            this.tblEmailToes = new HashSet<tblEmailTo>();
        }
    
        public System.Guid EmailGuid { get; set; }
        public System.Guid TemplateGuid { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailFrom { get; set; }
        public bool Sent { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid CreatedByGuid { get; set; }
    
        public virtual tblTemplate tblTemplate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmailTo> tblEmailToes { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}