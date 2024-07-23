using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectPrn.Models;

public partial class Prn212medicineContext : DbContext
{
    public static Prn212medicineContext INSTANCE = new Prn212medicineContext();
    public Prn212medicineContext()
    {
        if(INSTANCE == null)
        {
            INSTANCE = this;
        }
    }

    public Prn212medicineContext(DbContextOptions<Prn212medicineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<InfoAcc> InfoAccs { get; set; }

    public virtual DbSet<Medecine> Medecines { get; set; }

    public virtual DbSet<MedicineType> MedicineTypes { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<OrderHistoryDetail> OrderHistoryDetails { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    // => optionsBuilder.UseSqlServer("Data Source=HOA;Initial Catalog=PRN212Medicine; Trusted_Connection=SSPI;Encrypt=false");
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("value")); }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.MedicineId).HasColumnName("medicineId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_InfoAcc");

            entity.HasOne(d => d.Medicine).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Medecine");
        });

        modelBuilder.Entity<InfoAcc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Account");

            entity.ToTable("InfoAcc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.Wallet)
                .HasColumnType("money")
                .HasColumnName("wallet");

            entity.HasOne(d => d.Role).WithMany(p => p.InfoAccs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InfoAcc_Role");
        });

        modelBuilder.Entity<Medecine>(entity =>
        {
            entity.ToTable("Medecine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Decription).HasColumnName("decription");
            entity.Property(e => e.ExpiredDate).HasColumnName("expiredDate");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TypeId).HasColumnName("typeId");

            entity.HasOne(d => d.Type).WithMany(p => p.Medecines)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medecine_MedicineType");
        });

        modelBuilder.Entity<MedicineType>(entity =>
        {
            entity.ToTable("MedicineType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.ToTable("OrderHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.OrderDate).HasColumnName("orderDate");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod).HasColumnName("paymentMethod");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Account).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistory_InfoAcc");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.PaymentMethod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistory_PaymentMethod");

            entity.HasOne(d => d.StatusOrderNavigation).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistory_StatusOrder");
        });

        modelBuilder.Entity<OrderHistoryDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MedicineId).HasColumnName("medicineId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("money")
                .HasColumnName("purchasePrice");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Medicine).WithMany(p => p.OrderHistoryDetails)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistoryDetails_Medecine");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderHistoryDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistoryDetails_OrderHistory");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PaymentMethodName)
                .HasMaxLength(50)
                .HasColumnName("paymentMethodName");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_role");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.ToTable("StatusOrder");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("statusName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
