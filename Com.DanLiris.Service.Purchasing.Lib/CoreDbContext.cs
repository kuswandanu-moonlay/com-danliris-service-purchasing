using System;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Com.DanLiris.Service.Purchasing.Lib
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Divisions> Divisions { get; set; }
        public virtual DbSet<GarmentBuyers> GarmentBuyers { get; set; }
        public virtual DbSet<GarmentCategories> GarmentCategories { get; set; }
        public virtual DbSet<GarmentProducts> GarmentProducts { get; set; }
        public virtual DbSet<UnitOfMeasurements> UnitOfMeasurements { get; set; }
        public virtual DbSet<Units> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Divisions>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GarmentBuyers>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(3000);

                entity.Property(e => e.City).HasMaxLength(500);

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Contact).HasMaxLength(500);

                entity.Property(e => e.Country).HasMaxLength(500);

                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Npwp)
                    .HasColumnName("NPWP")
                    .HasMaxLength(100);

                entity.Property(e => e.Type).HasMaxLength(500);

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GarmentCategories>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CodeRequirement).HasMaxLength(255);

                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);

                entity.Property(e => e.UomUnit).HasMaxLength(255);
            });

            modelBuilder.Entity<GarmentProducts>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Const).HasMaxLength(500);

                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);

                entity.Property(e => e.UomUnit).HasMaxLength(500);

                entity.Property(e => e.Yarn).HasMaxLength(500);
            });

            modelBuilder.Entity<UnitOfMeasurements>(entity =>
            {
                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);

                entity.Property(e => e.Unit).HasMaxLength(500);
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.CreatedAgent)
                    .IsRequired()
                    .HasColumnName("_CreatedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("_CreatedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedUtc).HasColumnName("_CreatedUtc");

                entity.Property(e => e.DeletedAgent)
                    .IsRequired()
                    .HasColumnName("_DeletedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedBy)
                    .IsRequired()
                    .HasColumnName("_DeletedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.DeletedUtc).HasColumnName("_DeletedUtc");

                entity.Property(e => e.DivisionCode).HasMaxLength(100);

                entity.Property(e => e.DivisionName).HasMaxLength(500);

                entity.Property(e => e.IsDeleted).HasColumnName("_IsDeleted");

                entity.Property(e => e.LastModifiedAgent)
                    .IsRequired()
                    .HasColumnName("_LastModifiedAgent")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasColumnName("_LastModifiedBy")
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedUtc).HasColumnName("_LastModifiedUtc");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Uid)
                    .HasColumnName("UId")
                    .HasMaxLength(255);
            });
        }
    }
}
