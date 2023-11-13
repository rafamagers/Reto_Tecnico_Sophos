using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoWebAPI.Models;

[Table("MatriculaCurso")]
public partial class MatriculaCurso
{
    [Key]
    public int IdMatricula { get; set; }

    [Column("NRC")]
    public int? Nrc { get; set; }

    public int? CodigoEstudiantil { get; set; }

    public bool? Actual { get; set; }
}
