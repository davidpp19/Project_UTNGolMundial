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
    public class GruposController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;

        public GruposController(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        // GET: api/Grupos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupos()
        {
            return await _context.Grupos.ToListAsync();
        }

        // GET: api/Grupos/A
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(string id)
        {
            // Extraemos el primer caracter del string recibido en la URL
            char codigo = id.FirstOrDefault();
            var grupo = await _context.Grupos.FindAsync(codigo);

            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }

        // PUT: api/Grupos/A
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(string id, Grupo grupo)
        {
            char codigo = id.FirstOrDefault();

            if (codigo != grupo.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(grupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(codigo))
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

        // POST: api/Grupos
        [HttpPost]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrupo", new { id = grupo.Codigo }, grupo);
        }

        // DELETE: api/Grupos/A
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupo(string id)
        {
            char codigo = id.FirstOrDefault();
            var grupo = await _context.Grupos.FindAsync(codigo);
            if (grupo == null)
            {
                return NotFound();
            }

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GrupoExists(char id)
        {
            return _context.Grupos.Any(e => e.Codigo == id);
        }
    }
}