using System;
using System.Collections.Generic;
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
