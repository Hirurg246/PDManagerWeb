﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PDManagerWeb.Models;

#nullable disable

namespace PDManagerWeb.Migrations
{
    [DbContext(typeof(PDManagerContext))]
    [Migration("20240624181012_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PDManagerWeb.Models.AccessLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "UK_AccessLevel_Name")
                        .IsUnique();

                    b.ToTable("AccessLevels", t =>
                        {
                            t.HasTrigger("Trigger_AccessLevel_Delete");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("PDManagerWeb.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("isDeleted");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("login");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varbinary(32)")
                        .HasColumnName("passwordHash");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Login" }, "UK_Account_Login")
                        .IsUnique()
                        .HasFilter("([isDeleted]=(0))");

                    b.ToTable("Accounts", t =>
                        {
                            t.HasTrigger("Trigger_Account_Delete");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Login = "main_admin",
                            PasswordHash = new byte[] { 127, 65, 85, 90, 113, 253, 195, 203, 151, 18, 151, 23, 216, 26, 245, 115, 29, 24, 207, 127, 113, 169, 97, 79, 185, 3, 164, 42, 131, 223, 128, 36 }
                        });
                });

            modelBuilder.Entity("PDManagerWeb.Models.AllowedFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentFormatId")
                        .HasColumnType("int")
                        .HasColumnName("documentFormat_id");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int")
                        .HasColumnName("documentType_id");

                    b.HasKey("Id");

                    b.HasIndex("DocumentFormatId");

                    b.HasIndex(new[] { "DocumentTypeId", "DocumentFormatId" }, "UK_AllowedFormat_TypeFormat")
                        .IsUnique();

                    b.ToTable("AllowedFormats", t =>
                        {
                            t.HasTrigger("Trigger_AllowedFormat_Delete");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("PDManagerWeb.Models.DocumentFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("extension");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Extension" }, "UK_DocumentFormat_Extension")
                        .IsUnique();

                    b.ToTable("DocumentFormats");
                });

            modelBuilder.Entity("PDManagerWeb.Models.DocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "UK_DocumentType_Name")
                        .IsUnique();

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("PDManagerWeb.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("HeadId")
                        .HasColumnType("int")
                        .HasColumnName("head_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("isDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<DateOnly>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("startDate")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("Id");

                    b.HasIndex("HeadId");

                    b.HasIndex(new[] { "Name" }, "UK_Product_Name")
                        .IsUnique()
                        .HasFilter("([isDeleted]=(0))");

                    b.ToTable("Products", t =>
                        {
                            t.HasTrigger("Trigger_Product_Delete");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("PDManagerWeb.Models.ProductAccess", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<int>("AccessLevelId")
                        .HasColumnType("int")
                        .HasColumnName("accessLevel_id");

                    b.Property<bool>("IsGranted")
                        .HasColumnType("bit")
                        .HasColumnName("isGranted");

                    b.HasKey("ProductId", "AccountId");

                    b.HasIndex("AccessLevelId");

                    b.HasIndex("AccountId");

                    b.ToTable("ProductAccesses");
                });

            modelBuilder.Entity("PDManagerWeb.Models.ProgramDocument", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("DocumentTypeFormatId")
                        .HasColumnType("int")
                        .HasColumnName("documentTypeFormat_id");

                    b.Property<DateOnly>("LastChangeDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("lastChangeDate")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("LastChangeUserId")
                        .HasColumnType("int")
                        .HasColumnName("lastChangeUser_id");

                    b.HasKey("ProductId", "DocumentTypeFormatId");

                    b.HasIndex("DocumentTypeFormatId");

                    b.HasIndex("LastChangeUserId");

                    b.ToTable("ProgramDocuments");
                });

            modelBuilder.Entity("PDManagerWeb.Models.SysAdmin", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit")
                        .HasColumnName("isMain");

                    b.HasKey("Id");

                    b.ToTable("SysAdmins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsMain = true
                        });
                });

            modelBuilder.Entity("PDManagerWeb.Models.AllowedFormat", b =>
                {
                    b.HasOne("PDManagerWeb.Models.DocumentFormat", "DocumentFormat")
                        .WithMany("AllowedFormats")
                        .HasForeignKey("DocumentFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PDManagerWeb.Models.DocumentType", "DocumentType")
                        .WithMany("AllowedFormats")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentFormat");

                    b.Navigation("DocumentType");
                });

            modelBuilder.Entity("PDManagerWeb.Models.Product", b =>
                {
                    b.HasOne("PDManagerWeb.Models.Account", "Head")
                        .WithMany("Products")
                        .HasForeignKey("HeadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Head");
                });

            modelBuilder.Entity("PDManagerWeb.Models.ProductAccess", b =>
                {
                    b.HasOne("PDManagerWeb.Models.AccessLevel", "AccessLevel")
                        .WithMany("ProductAccesses")
                        .HasForeignKey("AccessLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PDManagerWeb.Models.Account", "Account")
                        .WithMany("ProductAccesses")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PDManagerWeb.Models.Product", "Product")
                        .WithMany("ProductAccesses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccessLevel");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PDManagerWeb.Models.ProgramDocument", b =>
                {
                    b.HasOne("PDManagerWeb.Models.AllowedFormat", "DocumentTypeFormat")
                        .WithMany("ProgramDocuments")
                        .HasForeignKey("DocumentTypeFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PDManagerWeb.Models.Account", "LastChangeUser")
                        .WithMany("ProgramDocuments")
                        .HasForeignKey("LastChangeUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PDManagerWeb.Models.Product", "Product")
                        .WithMany("ProgramDocuments")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentTypeFormat");

                    b.Navigation("LastChangeUser");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PDManagerWeb.Models.SysAdmin", b =>
                {
                    b.HasOne("PDManagerWeb.Models.Account", "IdNavigation")
                        .WithOne("SysAdmin")
                        .HasForeignKey("PDManagerWeb.Models.SysAdmin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("PDManagerWeb.Models.AccessLevel", b =>
                {
                    b.Navigation("ProductAccesses");
                });

            modelBuilder.Entity("PDManagerWeb.Models.Account", b =>
                {
                    b.Navigation("ProductAccesses");

                    b.Navigation("Products");

                    b.Navigation("ProgramDocuments");

                    b.Navigation("SysAdmin");
                });

            modelBuilder.Entity("PDManagerWeb.Models.AllowedFormat", b =>
                {
                    b.Navigation("ProgramDocuments");
                });

            modelBuilder.Entity("PDManagerWeb.Models.DocumentFormat", b =>
                {
                    b.Navigation("AllowedFormats");
                });

            modelBuilder.Entity("PDManagerWeb.Models.DocumentType", b =>
                {
                    b.Navigation("AllowedFormats");
                });

            modelBuilder.Entity("PDManagerWeb.Models.Product", b =>
                {
                    b.Navigation("ProductAccesses");

                    b.Navigation("ProgramDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
