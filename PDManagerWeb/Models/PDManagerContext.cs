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
        });

        modelBuilder.Entity<AllowedFormat>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_AllowedFormat_Delete"));
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trigger_Product_Delete"));

            entity.HasIndex(e => e.Name, "UK_Product_Name")
                .IsUnique()
                .HasFilter("([isDeleted]=(0))");

            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ProgramDocument>(entity =>
        {
            entity.Property(e => e.LastChangeDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<SysAdmin>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
