using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[Index("Name", Name = "UK_AccessLevel_Name", IsUnique = true)]
public partial class AccessLevel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(15)]
    public string Name { get; set; } = null!;

    [InverseProperty("AccessLevel")]
    public virtual ICollection<ProductAccess> ProductAccesses { get; set; } = new List<ProductAccess>();
}
