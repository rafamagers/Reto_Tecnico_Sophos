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
    [ApiController]
    [Authorize(Roles = "Admin,Maestro")]
    public class MateriumController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriumController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Materium
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materium>>> GetMateria()
        {
          if (_context.Materia == null)
          {
              return NotFound();
          }
            return await _context.Materia.ToListAsync();
        }

        // GET: api/Materium/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materium>> GetMaterium(int id)
        {
          if (_context.Materia == null)
          {
              return NotFound();
          }
            var materium = await _context.Materia.FindAsync(id);

            if (materium == null)
            {
                return NotFound();
            }

            return materium;
        }

        // PUT: api/Materium/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterium(int id, Materium materium)
        {
            if (id != materium.CodigoMateria)
            {
                return BadRequest();
            }

            _context.Entry(materium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriumExists(id))
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

        // POST: api/Materium
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Materium>> PostMaterium(Materium materium)
        {
          if (_context.Materia == null)
          {
              return Problem("Entity set 'AppDbContext.Materia'  is null.");
          }
            _context.Materia.Add(materium);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MateriumExists(materium.CodigoMateria))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMaterium", new { id = materium.CodigoMateria }, materium);
        }

        // DELETE: api/Materium/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterium(int id)
        {
            if (_context.Materia == null)
            {
                return NotFound();
            }
            var materium = await _context.Materia.FindAsync(id);
            if (materium == null)
            {
                return NotFound();
            }

            _context.Materia.Remove(materium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriumExists(int id)
        {
            return (_context.Materia?.Any(e => e.CodigoMateria == id)).GetValueOrDefault();
        }
    }
}
