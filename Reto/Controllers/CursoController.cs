using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using ProyectoWebAPI.Models;
using Reto.DBContext;
using Reto.Models;
using static Reto.Controllers.EstudianteController;

namespace Reto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Curso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoMateria>>> GetCursos()
        {
          if (_context.Cursos == null)
          {
              return NotFound();
          }
            var cursos = await _context.Cursos.ToListAsync();
            var CursoMateria = new List<Object>();
            foreach (var curso in cursos)
            {
                var materia = await _context.Materia
                    .Where(m => m.CodigoMateria.Equals(curso.CodigoMateria))
                    .FirstOrDefaultAsync();

                if (materia != null)
                {
                    var Prereq = await _context.Materia.FindAsync(materia.MateriaPrereq);
                    var cursoMateriaObj = new CursoMateria
                    {
                        NombreMateria = materia.Nombre,
                        Nrc = curso.Nrc,
                        CuposDisponibles = (int)curso.CuposDisponibles,
                        Facultad = materia.Facultad,
                        Idprofe = (int)curso.Idprofesor,
                        MateriaPrerequisito = Prereq,
                        NumeroCreditos = (int)materia.NumeroCreditos
                    };
                   
                    CursoMateria.Add(cursoMateriaObj);

                }
            }
            return Ok(CursoMateria);
        }


        // GET: api/Curso
        [HttpGet("PorCupos/{act}")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosCupo(bool act)
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            if (act)
            {
                var cursosdisp = await _context.Cursos
                    .Where(curso => curso.CuposDisponibles > 0) //Seleccionar aquellos cursos con cupos disponibles
                    .ToListAsync();
                return cursosdisp;
            }
            else {
                var cursosdisp = await _context.Cursos
                        .Where(curso => curso.CuposDisponibles == 0) //Seleccionar aquellos cursos con cupos disponibles
                        .ToListAsync();
                return cursosdisp;
            }
            
        }
        // GET: api/Curso/Compiladores
        // Metodo http para buscar cursos con un nombre especifico
        [HttpGet("obtenerPorNombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> ObtenerCursosPorNombre(string nombre)
        {

            var cursosConNombre = await _context.Cursos
                .Where(curso => _context.Materia.Any(materia => materia.Nombre.Contains(nombre) && materia.CodigoMateria == curso.CodigoMateria)) //Seleccionar aquellos cursos los cuales su materia contiene el nombre especificado
                .ToListAsync();

            if (cursosConNombre == null || cursosConNombre.Count == 0)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            return Ok(cursosConNombre);

        }
        // GET: api/Curso/5
        // Listar toda la información de un curso en específico
        [HttpGet("obtenerTODAINFO/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> ObtenerCursosPorID(int id)
        {
            //Encontrar el curso con el ID
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            //Encontrar el profesor asociado al curso
            var Profesor = await _context.Maestros.FindAsync(curso.Idprofesor);
            //Encontrar la materia asociada al curso
            var Materia = _context.Materia
                .Where(m => m.CodigoMateria.Equals(curso.CodigoMateria))
                .FirstOrDefault();
            //Obtener los códigos de los estudiantes matriculados en dicho curso
            var IdEstudiantes = _context.MatriculaCurso
              .Where(matricula => matricula.Nrc.Equals(curso.Nrc))
              .Select(matricula => matricula.CodigoEstudiantil)
              .ToList();
            //Obtener toda la información de los estudiantes con los códigos ya obtenidos
            var estudiantes = await _context.Estudiantes
                .Where(estudiante => IdEstudiantes.Contains(estudiante.CodigoEstudiantil))
                .ToListAsync();

            //Expandir información del curso obteniendo información de la materia asociada
            var cursosConNombreConEstudiantes = new
            {
                Curso = new { Nombre = Materia.Nombre, curso.Nrc, curso.CuposDisponibles, Materia.Facultad, Materia.MateriaPrereq, Materia.NumeroCreditos },
                Profesor = Profesor,
                NumEstudiantes = estudiantes.Count(),
                Estudiantes = estudiantes
                
            };

            return Ok(cursosConNombreConEstudiantes);

        }
        // PUT: api/Curso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.Nrc)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Curso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {

          if (_context.Cursos == null)
          {
              return Problem("Entity set 'AppDbContext.Cursos'  is null.");
          }
            var mat = await _context.Materia.FindAsync(curso.CodigoMateria);
            var prof = await _context.Maestros.FindAsync(curso.Idprofesor);
            //Asegurarse de crear un curso asociado a una materia y a un profesor existente
            if (mat == null || prof == null)
            {
                return NotFound();
            }
            else
            {
                _context.Cursos.Add(curso);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CursoExists(curso.Nrc))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
                return CreatedAtAction("GetCurso", new { id = curso.Nrc }, curso);
            }

           
        }

        // DELETE: api/Curso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            var matriculas = _context.MatriculaCurso
              .Where(matricula => matricula.Nrc.Equals(curso.Nrc))
              .ToList();
            foreach (var m in matriculas)
            {
                 _context.MatriculaCurso.Remove(m);
            }
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CursoExists(int id)
        {
            return (_context.Cursos?.Any(e => e.Nrc == id)).GetValueOrDefault();
        }
    }
}
