namespace Reto.Models
{
    public class CursoMateria
    {
        public string NombreMateria { get; set; }
        public int Nrc { get; set; }
        public int CuposDisponibles { get; set; }
        public string Facultad { get; set; }
        public int NumeroCreditos { get; set; }
        public int? Idprofe { get; set; }
        public Materium? MateriaPrerequisito { get; set; }
    }
}
