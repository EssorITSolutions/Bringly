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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class BringlyEntities : DbContext
    {
        public BringlyEntities()
            : base("name=BringlyEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserAddress> tblUserAddresses { get; set; }
        public virtual DbSet<tblLookUpDomain> tblLookUpDomains { get; set; }
        public virtual DbSet<tblLookUpDomainValue> tblLookUpDomainValues { get; set; }
        public virtual DbSet<tblCountry> tblCountries { get; set; }
        public virtual DbSet<tblFavourite> tblFavourites { get; set; }
        public virtual DbSet<tblItem> tblItems { get; set; }
        public virtual DbSet<tblRestaurant> tblRestaurants { get; set; }
    }
}