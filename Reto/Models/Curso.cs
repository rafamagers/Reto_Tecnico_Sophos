using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reto.Models;

public partial class Curso
{
    [Key]
    [Column("NRC")]
    public int Nrc { get; set; }

    public int? CodigoMateria { get; set; }

    public int? CuposDisponibles { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Semestre { get; set; }

    [Column("IDProfesor")]
    public int? Idprofesor { get; set; }

}
