using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

[Table("Admin")]
public partial class Admin
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Nombres { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Apellidos { get; set; }

    public int? Edad { get; set; }
}
