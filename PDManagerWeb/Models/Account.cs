using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace PDManagerWeb.Models;

public partial class Account
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("login")]
    [StringLength(20)]
    public string Login { get; set; } = null!;

    [Column("passwordHash")]
    [MaxLength(32)]
    public byte[] PasswordHash { get; set; } = null!;

    [Column("isDeleted")]
    public bool IsDeleted { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<ProductAccess> ProductAccesses { get; set; } = new List<ProductAccess>();

    [InverseProperty("Head")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("LastChangeUser")]
    public virtual ICollection<ProgramDocument> ProgramDocuments { get; set; } = new List<ProgramDocument>();

    [InverseProperty("IdNavigation")]
    public virtual SysAdmin? SysAdmin { get; set; }
}