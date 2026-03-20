using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Neveria.Models.dbFreezeDream;

public partial class DbFreezeDreamContext : DbContext
{
    public DbFreezeDreamContext()
    {
    }

    public DbFreezeDreamContext(DbContextOptions<DbFreezeDreamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:dbContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.TagCategorie).HasName("PK__Categori__6C2483870743CC95");

            entity.Property(e => e.DescriptionCategorie)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NameCategorie)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.TagEmployee).HasName("PK__Employee__5B2DDAF52BF4A08A");

            entity.HasIndex(e => e.TagUser, "UQ__Employee__44B07DCE4835DF11").IsUnique();

            entity.HasOne(d => d.TagUserNavigation).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.TagUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Users");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.TagInventory).HasName("PK__Inventor__1DBF9A20F2D03609");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.TagProduct, "UQ__Inventor__06057FFBB5BE3F77").IsUnique();

            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.TagProductNavigation).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.TagProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.TagProduct).HasName("PK__Products__06057FFA0648B48E");

            entity.Property(e => e.DescriptionProduct)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NameProduct)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.TagCategorieNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.TagCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.TagRole).HasName("PK__Role__59C3090BAFF39F6B");

            entity.ToTable("Role");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NameRole)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.TaglSale).HasName("PK__Sales__5D0DB2A15C15A980");

            entity.Property(e => e.DateSale).HasColumnType("datetime");
            entity.Property(e => e.DueTotal).HasColumnType("money");

            entity.HasOne(d => d.TagEmployeeNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.TagEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Employees");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.TagSaleDetail).HasName("PK__SaleDeta__25DB124AF70BDD19");

            entity.ToTable("SaleDetail");

            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.TagProductNavigation).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.TagProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Products");

            entity.HasOne(d => d.TaglSaleNavigation).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.TaglSale)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Sales");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.TagUser).HasName("PK__Users__44B07DCF549E32DE");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053442F2030F").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456785D2010").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.TagRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.TagRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
