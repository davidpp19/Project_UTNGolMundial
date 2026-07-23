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
    public class SeleccionesController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;
        private readonly Project_UTNGolMundial.Services.ITorneoValidacionService _validacionService;

        public SeleccionesController(MiApiUTNGolMundialContext context, Project_UTNGolMundial.Services.ITorneoValidacionService validacionService)
        {
            _context = context;
            _validacionService = validacionService;
        }

        // GET: api/Selecciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seleccion>>> GetSelecciones()
        {
            return await _context.Selecciones.ToListAsync();
        }

        // GET: api/Selecciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seleccion>> GetSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);

            if (seleccion == null)
            {
                return NotFound();
            }

            return seleccion;
        }

        // PUT: api/Selecciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeleccion(int id, Seleccion seleccion)
        {
            if (id != seleccion.Id)
            {
                return BadRequest();
            }

            if (seleccion.GrupoCodigo != default(char) && seleccion.GrupoCodigo != ' ')
            {
                try
                {
                    await _validacionService.ValidarLímitesGrupoAsync(seleccion.Id, seleccion.GrupoCodigo);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { mensaje = ex.Message });
                }
            }

            _context.Entry(seleccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeleccionExists(id))
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

        // POST: api/Selecciones
        [HttpPost]
        public async Task<ActionResult<Seleccion>> PostSeleccion(Seleccion seleccion)
        {
            if (seleccion.GrupoCodigo != default(char) && seleccion.GrupoCodigo != ' ')
            {
                try
                {
                    await _validacionService.ValidarLímitesGrupoAsync(seleccion.Id, seleccion.GrupoCodigo);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { mensaje = ex.Message });
                }
            }

            _context.Selecciones.Add(seleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeleccion", new { id = seleccion.Id }, seleccion);
        }

        // DELETE: api/Selecciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null)
            {
                return NotFound();
            }

            _context.Selecciones.Remove(seleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeleccionExists(int id)
        {
            return _context.Selecciones.Any(e => e.Id == id);
        }
    }
}