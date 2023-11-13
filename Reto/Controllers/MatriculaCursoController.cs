using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.Models;
using Reto.DBContext;

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

        // GET: api/MatriculaCurso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaCurso>>> GetMatriculaCurso()
        {
          if (_context.MatriculaCurso == null)
          {
              return NotFound();
          }
            return await _context.MatriculaCurso.ToListAsync();
        }

        // GET: api/MatriculaCurso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaCurso>> GetMatriculaCurso(int id)
        {
          if (_context.MatriculaCurso == null)
          {
              return NotFound();
          }
            var matriculaCurso = await _context.MatriculaCurso.FindAsync(id);

            if (matriculaCurso == null)
            {
                return NotFound();
            }

            return matriculaCurso;
        }

        // PUT: api/MatriculaCurso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatriculaCurso(int id, MatriculaCurso matriculaCurso)
        {
            if (id != matriculaCurso.IdMatricula)
            {
                return BadRequest();
            }

            _context.Entry(matriculaCurso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaCursoExists(id))
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

        // POST: api/MatriculaCurso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MatriculaCurso>> PostMaterium(MatriculaCurso MatriculaCurso)
        {
            if (_context.MatriculaCurso == null)
            {
                return Problem("Entity set 'AppDbContext.Materia'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(MatriculaCurso.Nrc);
            var estudiante = await _context.Estudiantes.FindAsync(MatriculaCurso.CodigoEstudiantil);
            if (curso == null || estudiante == null)
            {
                return NotFound();
            }
            else
            {


                _context.MatriculaCurso.Add(MatriculaCurso);
                curso.CuposDisponibles = curso.CuposDisponibles - 1;

                await _context.SaveChangesAsync();



                return CreatedAtAction("GetMatriculaCurso", new { id = MatriculaCurso.IdMatricula }, MatriculaCurso);
            }
        }

        // DELETE: api/MatriculaCurso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatriculaCurso(int id)
        {
            if (_context.MatriculaCurso == null)
            {
                return NotFound();
            }
            var matriculaCurso = await _context.MatriculaCurso.FindAsync(id);
            if (matriculaCurso == null)
            {
                return NotFound();
            }

            _context.MatriculaCurso.Remove(matriculaCurso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatriculaCursoExists(int id)
        {
            return (_context.MatriculaCurso?.Any(e => e.IdMatricula == id)).GetValueOrDefault();
        }
    }
}
