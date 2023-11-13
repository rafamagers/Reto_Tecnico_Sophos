using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reto.DBContext;
using Reto.Models;

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
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
          if (_context.Cursos == null)
          {
              return NotFound();
          }
            return await _context.Cursos.ToListAsync();
        }

        // GET: api/Curso/5
        [HttpGet("obtenerPorNombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> ObtenerCursosPorNombre(string nombre)
        {
            var cursosConNombre = _context.Cursos
                .Where(curso => _context.Materia.Any(materia => materia.Nombre.Contains(nombre) && materia.CodigoMateria == curso.CodigoMateria))
                .ToList();

            if (cursosConNombre == null || cursosConNombre.Count == 0)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            return Ok(cursosConNombre);

        }
        // GET: api/Curso/5
        [HttpGet("obtenerTODAINFO/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> ObtenerCursosPorID(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            var Profesor = await _context.Maestros.FindAsync(curso.Idprofesor);
            var Materia = _context.Materia
                .Where(m => m.CodigoMateria.Equals(curso.CodigoMateria))
                .FirstOrDefault();
            var IdEstudiantes = _context.MatriculaCurso
              .Where(matricula => matricula.Nrc.Equals(curso.Nrc))
              .Select(matricula => matricula.CodigoEstudiantil)
              .ToList();

           
            var estudiantes = await _context.Estudiantes
                .Where(estudiante => IdEstudiantes.Contains(estudiante.CodigoEstudiantil))
                .ToListAsync();


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
