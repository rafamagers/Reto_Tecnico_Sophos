using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Reto.DBContext;
using Reto.Models;

namespace Reto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudianteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudiante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
          if (_context.Estudiantes == null)
          {
              return NotFound();
          }
            return await _context.Estudiantes.ToListAsync();
        }


        // GET: api/Estudiante/5
        [HttpGet("obtenerTODAINFO/{id}")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> ObtenerINFO(int id)
        {
            var Estudiante = await _context.Estudiantes.FindAsync(id);
            if (Estudiante == null)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            var cursos = await _context.Cursos
                .Where(curso => _context.MatriculaCurso
                    .Any(matricula => matricula.Nrc.Equals(curso.Nrc) && matricula.CodigoEstudiantil.Equals(Estudiante.CodigoEstudiantil)))
                .Select(curso => new
                {
                    Curso = curso,
                    Actual = _context.MatriculaCurso
                        .Where(matricula => matricula.Nrc.Equals(curso.Nrc) && matricula.CodigoEstudiantil.Equals(Estudiante.CodigoEstudiantil))
                        .Select(matricula => matricula.Actual)
                        .FirstOrDefault()
                })
                .ToListAsync();
            var CursoMateriaNTList = new List<CursoMateria>();
            var CursoMateriaTList = new List<CursoMateria>();
            foreach (var curso in cursos)
            {
                var materia = await _context.Materia
                    .Where(m => m.CodigoMateria.Equals(curso.Curso.CodigoMateria))
                    .FirstOrDefaultAsync();

                if (materia != null)
                {
                    var Prereq = await _context.Materia.FindAsync(materia.MateriaPrereq);
                    var cursoMateriaObj = new CursoMateria
                    {
                        NombreMateria = materia.Nombre,
                        Nrc = curso.Curso.Nrc,
                        CuposDisponibles = (int)curso.Curso.CuposDisponibles,
                        Facultad = materia.Facultad,
                        Idprofe = (int)curso.Curso.Idprofesor,
                        MateriaPrerequisito = Prereq,
                        NumeroCreditos = (int)materia.NumeroCreditos
                    };
                    if (curso.Actual.Value == true)
                    {
                        CursoMateriaNTList.Add(cursoMateriaObj);
                    }
                    else
                    {
                        CursoMateriaTList.Add(cursoMateriaObj);
                    }
                }
            }
            var cursosConNombreConEstudiantes = new
            {
                Estudiante = Estudiante,
               
                NumCursos = cursos.Count(),
                CursosTerminados = CursoMateriaTList,
                CursosNoTerminados = CursoMateriaNTList

            };

            return Ok(cursosConNombreConEstudiantes);
        }
        // GET: api/Estudiante/5
        [HttpGet("obtenerPorNombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> ObtenerAlumnosPorNombre(string nombre)
        {
            // Utiliza el método Where para filtrar los resultados por nombre
            var alumnosConNombre =  _context.Estudiantes
                .Where(alumno => alumno.Nombres.Contains(nombre))
                .ToList();

            if (alumnosConNombre == null || alumnosConNombre.Count == 0)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            return Ok(alumnosConNombre);
        }
        // GET: api/Estudiante/5
        [HttpGet("obtenerPorFacultad/{facultad}")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> ObtenerAlumnosPorFacultad(string facultad)
        {
            // Utiliza el método Where para filtrar los resultados por nombre
            var alumnosConNombre = _context.Estudiantes
                .Where(alumno => alumno.Facultad.Contains(facultad))
                .ToList();

            if (alumnosConNombre == null || alumnosConNombre.Count == 0)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            return Ok(alumnosConNombre);
        }
        // PUT: api/Estudiante/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.CodigoEstudiantil)
            {
                return BadRequest();
            }

            _context.Entry(estudiante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudianteExists(id))
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

        // POST: api/Estudiante
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
          if (_context.Estudiantes == null)
          {
              return Problem("Entity set 'AppDbContext.Estudiantes'  is null.");
          }
            _context.Estudiantes.Add(estudiante);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstudianteExists(estudiante.CodigoEstudiantil))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEstudiante", new { id = estudiante.CodigoEstudiantil }, estudiante);
        }

        // DELETE: api/Estudiante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            if (_context.Estudiantes == null)
            {
                return NotFound();
            }
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            
            var matriculas = _context.MatriculaCurso
              .Where(matricula => matricula.CodigoEstudiantil.Equals(estudiante.CodigoEstudiantil))
              .ToList();
            foreach (var m in matriculas)
            {
                var curso = await _context.Cursos.FindAsync(m.Nrc);
                curso.CuposDisponibles = curso.CuposDisponibles + 1;
                _context.MatriculaCurso.Remove(m);
            }
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudianteExists(int id)
        {
            return (_context.Estudiantes?.Any(e => e.CodigoEstudiantil == id)).GetValueOrDefault();
        }
    }
}
