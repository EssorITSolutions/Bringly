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
    
    public partial class tblEmailTo
    {
        public System.Guid EmailToGuid { get; set; }
        public string EmailTo { get; set; }
        public System.Guid EmailGuid { get; set; }
        public bool Read { get; set; }
    
        public virtual tblEmail tblEmail { get; set; }
    }
}