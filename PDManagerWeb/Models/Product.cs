using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("head_id")]
    public int HeadId { get; set; }

    [Column("startDate")]
    public DateOnly StartDate { get; set; }

    [Column("isDeleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey("HeadId")]
    [InverseProperty("Products")]
    public virtual Account Head { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<ProductAccess> ProductAccesses { get; set; } = new List<ProductAccess>();

    [InverseProperty("Product")]
    public virtual ICollection<ProgramDocument> ProgramDocuments { get; set; } = new List<ProgramDocument>();
}
