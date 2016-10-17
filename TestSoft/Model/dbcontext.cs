namespace TestSoft.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbcontext : DbContext
    {
        public dbcontext()
            : base("name=dbcontext")
        {
        }

        public virtual DbSet<bomm> bomm { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bomm>()
                .Property(e => e.bom_level)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Parent_Part_Number)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Part_Number)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Part_Name)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Revision)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Quantit)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Unit_of_measure)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Procurement_Type)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.Reference_Designatos)
                .IsUnicode(false);

            modelBuilder.Entity<bomm>()
                .Property(e => e.BOM_Notes)
                .IsUnicode(false);
        }
    }
}
