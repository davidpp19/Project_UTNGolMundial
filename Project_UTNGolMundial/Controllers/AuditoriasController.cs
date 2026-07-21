using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriasController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;

        public AuditoriasController(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        // GET: api/Auditorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auditoria>>> GetAuditorias()
        {
            return await _context.Auditorias.ToListAsync();
        }

        // GET: api/Auditorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auditoria>> GetAuditoria(long id)
        {
            var auditoria = await _context.Auditorias.FindAsync(id);

            if (auditoria == null)
            {
                return NotFound();
            }

            return auditoria;
        }

        // PUT: api/Auditorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuditoria(long id, Auditoria auditoria)
        {
            if (id != auditoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(auditoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditoriaExists(id))
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

        // POST: api/Auditorias
        [HttpPost]
        public async Task<ActionResult<Auditoria>> PostAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuditoria", new { id = auditoria.Id }, auditoria);
        }

        // DELETE: api/Auditorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditoria(long id)
        {
            var auditoria = await _context.Auditorias.FindAsync(id);
            if (auditoria == null)
            {
                return NotFound();
            }

            _context.Auditorias.Remove(auditoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuditoriaExists(long id)
        {
            return _context.Auditorias.Any(e => e.Id == id);
        }
    }
}