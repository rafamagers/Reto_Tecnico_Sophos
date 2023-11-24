using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reto.DBContext;
using Reto.Models;

namespace Reto.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Maestro")]
    [ApiController]
    public class MaestroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaestroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Maestro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maestro>>> GetMaestros()
        {
          if (_context.Maestros == null)
          {
              return NotFound();
          }
            return await _context.Maestros.ToListAsync();
        }

        // GET: api/Maestro/5
        [HttpGet("obtenerPorNombre/{nombre}")]
        public ActionResult<IEnumerable<Estudiante>> ObtenerMaestrosPorNombre(string nombre)
        {
            // Utiliza el método Where para filtrar los resultados por nombre
            var MaestrosConNombre = _context.Maestros
                .Where(Maestros => Maestros.Nombres.Contains(nombre))
                .ToList();

            if (MaestrosConNombre == null || MaestrosConNombre.Count == 0)
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }

            return Ok(MaestrosConNombre);
        }

        // PUT: api/Maestro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaestro(int id, Maestro maestro)
        {
            if (id != maestro.CodigoMaestro)
            {
                return BadRequest();
            }

            _context.Entry(maestro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaestroExists(id))
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

        // POST: api/Maestro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Maestro>> PostMaestro(Maestro maestro)
        {
          if (_context.Maestros == null)
          {
              return Problem("Entity set 'AppDbContext.Maestros'  is null.");
          }
            _context.Maestros.Add(maestro);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MaestroExists(maestro.CodigoMaestro))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMaestro", new { id = maestro.CodigoMaestro }, maestro);
        }

        // DELETE: api/Maestro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaestro(int id)
        {
            if (_context.Maestros == null)
            {
                return NotFound();
            }
            var maestro = await _context.Maestros.FindAsync(id);
            if (maestro == null)
            {
                return NotFound();
            }
            var Cursos = _context.Cursos
                .Where(c => c.Idprofesor.Equals(maestro.CodigoMaestro))
                .ToList();
            foreach (var c in Cursos){
                c.Idprofesor = null;
            }
            _context.Maestros.Remove(maestro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaestroExists(int id)
        {
            return (_context.Maestros?.Any(e => e.CodigoMaestro == id)).GetValueOrDefault();
        }
    }
}
