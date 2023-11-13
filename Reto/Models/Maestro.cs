using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

[Table("Maestro")]
[Index("Email", Name = "UQ__Maestro__A9D10534653D1A03", IsUnique = true)]
public partial class Maestro
{
    [Key]
    public int CodigoMaestro { get; set; }

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

    [InverseProperty("IdprofesorNavigation")]
    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
