using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class BCRM_58_Entities : DbContext
    {
        public BCRM_58_Entities()
        {
        }

        public BCRM_58_Entities(DbContextOptions<BCRM_58_Entities> options)
            : base(options)
        {
        }

        public virtual DbSet<Banner_Banner> Banner_Banners { get; set; }
        public virtual DbSet<Banner_Dynamic_Column_Mapping_Banner> Banner_Dynamic_Column_Mapping_Banners { get; set; }
        public virtual DbSet<Banner_Group_Banner> Banner_Group_Banners { get; set; }
        public virtual DbSet<Banner_Mapping_Group_Banner> Banner_Mapping_Group_Banners { get; set; }
        public virtual DbSet<Banner_Mapping_Set_Banner> Banner_Mapping_Set_Banners { get; set; }
        public virtual DbSet<Banner_Set_Banner> Banner_Set_Banners { get; set; }
        public virtual DbSet<CRM_Customer> CRM_Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=dev-chocobcrm.database.windows.net;Initial Catalog=BCRM_58_B50OHHD85Y0N;Persist Security Info=True;User ID=bcrm_B50OHHD85Y0N;password=lqfTnbRhP%QZGV@*FWia;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Banner_Banner>(entity =>
            {
                entity.HasKey(e => e.BannerId)
                    .HasName("PK_Banner");

                entity.ToTable("Banner_Banner");

                entity.Property(e => e.Banner_Image_BCI_Ref).HasMaxLength(250);

                entity.Property(e => e.Banner_Image_Image_Url).HasMaxLength(1000);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Layout_Section).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Note).HasMaxLength(250);

                entity.Property(e => e.Path).HasMaxLength(1000);

                entity.Property(e => e.Valid_From).HasColumnType("datetime");

                entity.Property(e => e.Valid_Through).HasColumnType("datetime");
            });

            modelBuilder.Entity<Banner_Dynamic_Column_Mapping_Banner>(entity =>
            {
                entity.HasKey(e => e.MappingId);

                entity.ToTable("Banner_Dynamic_Column_Mapping_Banner");

                entity.Property(e => e.Column_Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Dyn_Sys_Column).HasMaxLength(128);
            });

            modelBuilder.Entity<Banner_Group_Banner>(entity =>
            {
                entity.HasKey(e => e.Group_BannerId);

                entity.ToTable("Banner_Group_Banner");

                entity.Property(e => e.Group_Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Banner_Mapping_Group_Banner>(entity =>
            {
                entity.HasKey(e => e.MappingId);

                entity.ToTable("Banner_Mapping_Group_Banner");

                entity.Property(e => e.Type_Desc)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Banner_Mapping_Set_Banner>(entity =>
            {
                entity.HasKey(e => e.MappingId);

                entity.ToTable("Banner_Mapping_Set_Banner");
            });

            modelBuilder.Entity<Banner_Set_Banner>(entity =>
            {
                entity.HasKey(e => e.Set_BannerId);

                entity.ToTable("Banner_Set_Banner");

                entity.Property(e => e.Layout_Section).HasMaxLength(100);

                entity.Property(e => e.Layout_Type_Desc).HasMaxLength(50);

                entity.Property(e => e.Set_Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<CRM_Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("CRM_Customer");

                entity.Property(e => e.Avg_Spending).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CRM_Wallet_Alt_Ref).HasMaxLength(50);

                entity.Property(e => e.Customer_Ref)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.First_Name_En).HasMaxLength(250);

                entity.Property(e => e.First_Name_Th).HasMaxLength(250);

                entity.Property(e => e.Gov_Id_Ref).HasMaxLength(250);

                entity.Property(e => e.Identity_SRef)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Last_Name_En).HasMaxLength(250);

                entity.Property(e => e.Last_Name_Th).HasMaxLength(250);

                entity.Property(e => e.MBS_Tier_En).HasMaxLength(250);

                entity.Property(e => e.MBS_Tier_Th).HasMaxLength(250);

                entity.Property(e => e.Middle_Name_En).HasMaxLength(250);

                entity.Property(e => e.Middle_Name_Th).HasMaxLength(250);

                entity.Property(e => e.MobileNo).HasMaxLength(20);

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Registered_Channel).HasMaxLength(50);

                entity.Property(e => e.Spending).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title_En).HasMaxLength(250);

                entity.Property(e => e.Title_Th).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
