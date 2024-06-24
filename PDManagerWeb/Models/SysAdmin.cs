using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDManagerWeb.Models;

public partial class SysAdmin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("isMain")]
    public bool IsMain { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("SysAdmin")]
    public virtual Account IdNavigation { get; set; } = null!;
}
