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


}
