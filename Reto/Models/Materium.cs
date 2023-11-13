using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

public partial class Materium
{
    [Key]
    public int CodigoMateria { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    public int? MateriaPrereq { get; set; }

    public int? NumeroCreditos { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Facultad { get; set; }

    [InverseProperty("CodigoMateriaNavigation")]
    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    [InverseProperty("MateriaPrereqNavigation")]
    public virtual ICollection<Materium> InverseMateriaPrereqNavigation { get; set; } = new List<Materium>();

    [ForeignKey("MateriaPrereq")]
    [InverseProperty("InverseMateriaPrereqNavigation")]
    public virtual Materium? MateriaPrereqNavigation { get; set; }
}
