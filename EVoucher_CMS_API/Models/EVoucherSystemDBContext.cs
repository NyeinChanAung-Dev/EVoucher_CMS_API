using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EVoucher_CMS_API.Models
{
    public partial class EVoucherSystemDBContext : DbContext
    {
        public EVoucherSystemDBContext()
        {
        }

        public EVoucherSystemDBContext(DbContextOptions<EVoucherSystemDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuyTypeTb> BuyTypeTb { get; set; }
        public virtual DbSet<EvoucherTb> EvoucherTb { get; set; }
        public virtual DbSet<GeneratedEvoucherTb> GeneratedEvoucherTb { get; set; }
        public virtual DbSet<PaymentMethodTb> PaymentMethodTb { get; set; }
        public virtual DbSet<PurchaseHistoryTb> PurchaseHistoryTb { get; set; }
        public virtual DbSet<PurchaseOrderTb> PurchaseOrderTb { get; set; }
        public virtual DbSet<RefreshTokenTb> RefreshTokenTb { get; set; }
        public virtual DbSet<UsersTb> UsersTb { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-01F9C8A;Initial Catalog=EVoucherSystemDB;Persist Security Info=True;User ID=sa;Password=2good2btrue;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BuyTypeTb>(entity =>
            {
                entity.Property(e => e.BuyType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<EvoucherTb>(entity =>
            {
                entity.HasKey(e => e.VoucherNo);

                entity.ToTable("EVoucherTb");

                entity.Property(e => e.VoucherNo).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ImagePath).HasMaxLength(100);

                entity.Property(e => e.PaymentMethod).HasMaxLength(10);

                entity.Property(e => e.SellingPrice).HasColumnType("money");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VoucherAmount).HasColumnType("money");
            });

            modelBuilder.Entity<GeneratedEvoucherTb>(entity =>
            {
                entity.HasKey(e => e.PromoCode);

                entity.ToTable("GeneratedEVoucherTb");

                entity.Property(e => e.PromoCode).HasMaxLength(11);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.OwnerName).HasMaxLength(300);

                entity.Property(e => e.OwnerPhone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PurchaseOrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.QrimagePath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("QRImagePath");

                entity.Property(e => e.VoncherAmount).HasColumnType("money");

                entity.Property(e => e.VoucherImagePath)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VoucherNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<PaymentMethodTb>(entity =>
            {
                entity.HasKey(e => e.PaymentMethod);

                entity.Property(e => e.PaymentMethod).HasMaxLength(10);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PurchaseHistoryTb>(entity =>
            {
                entity.HasKey(e => e.PurchaseHistoryId);

                entity.Property(e => e.PromoCode).HasMaxLength(11);

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.PurchaseOrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.VoucherNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<PurchaseOrderTb>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderNo);

                entity.Property(e => e.PurchaseOrderNo).HasMaxLength(20);

                entity.Property(e => e.BuyType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.BuyerName).HasMaxLength(300);

                entity.Property(e => e.BuyerPhone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ImagePath).HasMaxLength(100);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.SellingPrice).HasColumnType("money");

                entity.Property(e => e.TotalSellingAmount).HasColumnType("money");

                entity.Property(e => e.VoncherAmount).HasColumnType("money");

                entity.Property(e => e.VoucherNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RefreshTokenTb>(entity =>
            {
                entity.HasKey(e => e.RefreshToken);

                entity.Property(e => e.RefreshToken).HasMaxLength(300);

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiry_Date");

                entity.Property(e => e.RefreshTokenId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RefreshToken_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");
            });

            modelBuilder.Entity<UsersTb>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(1000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
