using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

[PrimaryKey("ProductId", "DocumentTypeFormatId")]
public partial class ProgramDocument
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Key]
    [Column("documentTypeFormat_id")]
    public int DocumentTypeFormatId { get; set; }

    [Column("lastChangeDate")]
    public DateOnly LastChangeDate { get; set; }

    [Column("lastChangeUser_id")]
    public int LastChangeUserId { get; set; }

    [ForeignKey("DocumentTypeFormatId")]
    [InverseProperty("ProgramDocuments")]
    public virtual AllowedFormat DocumentTypeFormat { get; set; } = null!;

    [ForeignKey("LastChangeUserId")]
    [InverseProperty("ProgramDocuments")]
    public virtual Account LastChangeUser { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProgramDocuments")]
    public virtual Product Product { get; set; } = null!;
}
