using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[Index("Name", Name = "UK_DocumentType_Name", IsUnique = true)]
public partial class DocumentType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("DocumentType")]
    public virtual ICollection<AllowedFormat> AllowedFormats { get; set; } = new List<AllowedFormat>();
}
