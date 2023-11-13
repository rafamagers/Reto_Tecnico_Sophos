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
    public class MatriculaCursoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatriculaCursoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MateriaCurso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaCurso>>> GetMatricula()
        {
            if (_context.Materia == null)
            {
                return NotFound();
            }
            return await _context.MatriculaCursos.ToListAsync();
        }

        // GET: api/MatriculaCurso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaCurso>> GetMatricula(int id)
        {
            if (_context.MatriculaCursos == null)
            {
                return NotFound();
            }
            var MatriculaCurso = await _context.MatriculaCursos.FindAsync(id);

            if (MatriculaCurso == null)
            {
                return NotFound();
            }

            return MatriculaCurso;
        }


        // POST: api/MatriculaCurso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MatriculaCurso>> PostMaterium(MatriculaCurso MatriculaCurso)
        {
            if (_context.MatriculaCursos == null)
            {
                return Problem("Entity set 'AppDbContext.Materia'  is null.");
            }
            _context.MatriculaCursos.Add(MatriculaCurso);
    
               await _context.SaveChangesAsync();
            


            return CreatedAtAction("GetMatriculaCurso", new { id = MatriculaCurso.CodigoEstudiantil }, MatriculaCurso);
        }

        // DELETE: api/MatriculaCurso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatriculaCurso(int id)
        {
            if (_context.MatriculaCursos == null)
            {
                return NotFound();
            }
            var MatriculaCurso = await _context.MatriculaCursos.FindAsync(id);
            if (MatriculaCurso == null)
            {
                return NotFound();
            }

            _context.MatriculaCursos.Remove(MatriculaCurso);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
