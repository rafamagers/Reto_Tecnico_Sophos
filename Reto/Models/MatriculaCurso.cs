using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

[Keyless]
[Table("MatriculaCurso")]
public partial class MatriculaCurso
{
    [Column("NRC")]
    public int? Nrc { get; set; }

    public int? CodigoEstudiantil { get; set; }

    public bool? Actual { get; set; }

    [ForeignKey("CodigoEstudiantil")]
    public virtual Estudiante? CodigoEstudiantilNavigation { get; set; }

    [ForeignKey("Nrc")]
    public virtual Curso? NrcNavigation { get; set; }
}
