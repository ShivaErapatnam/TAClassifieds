﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAClassifieds.Models.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TAC_Team4Entities : DbContext
    {
        public TAC_Team4Entities()
            : base("name=TAC_Team4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TAC_Category> TAC_Category { get; set; }
        public virtual DbSet<TAC_Classified> TAC_Classified { get; set; }
        public virtual DbSet<TAC_ClassifiedContact> TAC_ClassifiedContact { get; set; }
        public virtual DbSet<TAC_Country> TAC_Country { get; set; }
        public virtual DbSet<TAC_User> TAC_User { get; set; }
        public virtual DbSet<TAC_ContactUs> TAC_ContactUs { get; set; }
    }
}
