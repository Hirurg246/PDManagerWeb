using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

public partial class PDManagerContext : DbContext
{
    public PDManagerContext()
    {
    }

    public PDManagerContext(DbContextOptions<PDManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLevel> AccessLevels { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AllowedFormat> AllowedFormats { get; set; }

    public virtual DbSet<DocumentFormat> DocumentFormats { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAccess> ProductAccesses { get; set; }

    public virtual DbSet<ProgramDocument> ProgramDocuments { get; set; }

    public virtual DbSet<SysAdmin> SysAdmins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBHSE_PDManager");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLevel>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_AccessLevel_Delete"));
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_Account_Delete"));

            entity.HasIndex(e => e.Login, "UK_Account_Login")
                .IsUnique()
                .HasFilter("([isDeleted]=(0))");

            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.PasswordHash).IsFixedLength();
        });

        modelBuilder.Entity<AllowedFormat>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_AllowedFormat_Delete"));

            //entity.HasOne(d => d.DocumentFormat).WithMany(p => p.AllowedFormats).HasConstraintName("FK_AllowedFormat_DocumentFormat");

            //entity.HasOne(d => d.DocumentType).WithMany(p => p.AllowedFormats).HasConstraintName("FK_AllowedFormat_DocumentType");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_Product_Delete"));

            entity.HasIndex(e => e.Name, "UK_Product_Name")
                .IsUnique()
                .HasFilter("([isDeleted]=(0))");

            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Head).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Head");
        });

        modelBuilder.Entity<ProductAccess>(entity =>
        {
            entity.HasOne(d => d.AccessLevel).WithMany(p => p.ProductAccesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductAccesse_AccessLevel");

            //entity.HasOne(d => d.Account).WithMany(p => p.ProductAccesses).HasConstraintName("FK_ProductAccess_Account");

            //entity.HasOne(d => d.Product).WithMany(p => p.ProductAccesses).HasConstraintName("FK_ProductAccess_Product");
        });

        modelBuilder.Entity<ProgramDocument>(entity =>
        {
            entity.Property(e => e.LastChangeDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.DocumentTypeFormat).WithMany(p => p.ProgramDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProgramDocument_TypeFormat");

            //entity.HasOne(d => d.LastChangeUser).WithMany(p => p.ProgramDocuments).HasConstraintName("FK_ProgramDocument_ChangeUser");

            //entity.HasOne(d => d.Product).WithMany(p => p.ProgramDocuments).HasConstraintName("FK_ProgramDocument_Product");
        });

        modelBuilder.Entity<SysAdmin>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.SysAdmin).HasConstraintName("FK_SysAdmin_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
