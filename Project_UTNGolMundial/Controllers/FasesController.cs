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
    public class FasesController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;

        public FasesController(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        // GET: api/Fases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fase>>> GetFases()
        {
            return await _context.Fases.ToListAsync();
        }

        // GET: api/Fases/GRUPOS
        [HttpGet("{id}")]
        public async Task<ActionResult<Fase>> GetFase(string id)
        {
            var fase = await _context.Fases.FindAsync(id);

            if (fase == null)
            {
                return NotFound();
            }

            return fase;
        }

        // PUT: api/Fases/GRUPOS
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFase(string id, Fase fase)
        {
            if (id != fase.Codigo) // Fase usa 'Codigo' como Key de tipo string
            {
                return BadRequest();
            }

            _context.Entry(fase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaseExists(id))
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

        // POST: api/Fases
        [HttpPost]
        public async Task<ActionResult<Fase>> PostFase(Fase fase)
        {
            _context.Fases.Add(fase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFase", new { id = fase.Codigo }, fase);
        }

        // DELETE: api/Fases/GRUPOS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFase(string id)
        {
            var fase = await _context.Fases.FindAsync(id);
            if (fase == null)
            {
                return NotFound();
            }

            _context.Fases.Remove(fase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FaseExists(string id)
        {
            return _context.Fases.Any(e => e.Codigo == id);
        }
    }
}