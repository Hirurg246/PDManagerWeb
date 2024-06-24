using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[Index("DocumentTypeId", "DocumentFormatId", Name = "UK_AllowedFormat_TypeFormat", IsUnique = true)]
public partial class AllowedFormat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("documentType_id")]
    public int DocumentTypeId { get; set; }

    [Column("documentFormat_id")]
    public int DocumentFormatId { get; set; }

    [ForeignKey("DocumentFormatId")]
    [InverseProperty("AllowedFormats")]
    public virtual DocumentFormat DocumentFormat { get; set; } = null!;

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("AllowedFormats")]
    public virtual DocumentType DocumentType { get; set; } = null!;

    [InverseProperty("DocumentTypeFormat")]
    public virtual ICollection<ProgramDocument> ProgramDocuments { get; set; } = new List<ProgramDocument>();
}
