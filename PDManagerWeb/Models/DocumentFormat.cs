using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[Index("Extension", Name = "UK_DocumentFormat_Extension", IsUnique = true)]
public partial class DocumentFormat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("extension")]
    [StringLength(10)]
    public string Extension { get; set; } = null!;

    [InverseProperty("DocumentFormat")]
    public virtual ICollection<AllowedFormat> AllowedFormats { get; set; } = new List<AllowedFormat>();
}
