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
    
    public partial class tblFavourite
    {
        public System.Guid FavouriteGuid { get; set; }
        public System.Guid LocationGuid { get; set; }
        public System.Guid CreatedByGuid { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual tblLocation tblLocation { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
