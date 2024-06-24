using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[PrimaryKey("ProductId", "AccountId")]
public partial class ProductAccess
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Key]
    [Column("account_id")]
    public int AccountId { get; set; }

    [Column("isGranted")]
    public bool IsGranted { get; set; }

    [Column("accessLevel_id")]
    public int AccessLevelId { get; set; }

    [ForeignKey("AccessLevelId")]
    [InverseProperty("ProductAccesses")]
    public virtual AccessLevel AccessLevel { get; set; } = null!;

    [ForeignKey("AccountId")]
    [InverseProperty("ProductAccesses")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductAccesses")]
    public virtual Product Product { get; set; } = null!;
}
