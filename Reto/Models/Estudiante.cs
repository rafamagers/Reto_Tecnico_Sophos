using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

[Table("Estudiante")]
[Index("Email", Name = "UQ__Estudian__A9D10534DCF3DE57", IsUnique = true)]
public partial class Estudiante
{
    [Key]
    public int CodigoEstudiantil { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Nombres { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Apellidos { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    public int? Edad { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Facultad { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SemestreActual { get; set; }
}
