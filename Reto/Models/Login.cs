using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

[Table("Login")]
public partial class Login
{
    public int? IdLogin { get; set; }

    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Password { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TypeUser { get; set; }
}
