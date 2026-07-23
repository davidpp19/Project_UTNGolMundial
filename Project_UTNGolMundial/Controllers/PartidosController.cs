using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;
using Project_UTNGolMundial.DTOs;
using Project_UTNGolMundial.Services;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidosController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;
        private readonly IPartidoResultadoService _resultadoService;
        private readonly IEstadisticasService _estadisticasService;
        private readonly ITorneoValidacionService _validacionService;

        public PartidosController(
            MiApiUTNGolMundialContext context, 
            IPartidoResultadoService resultadoService,
            IEstadisticasService estadisticasService,
            ITorneoValidacionService validacionService)
        {
            _context = context;
            _resultadoService = resultadoService;
            _estadisticasService = estadisticasService;
            _validacionService = validacionService;
        }

        // GET: api/Partidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partido>>> GetPartidos()
        {
            return await _context.Partidos.ToListAsync();
        }

        // GET: api/Partidos/calendario
        [HttpGet("calendario")]
        public async Task<ActionResult<List<CalendarioPartidoDto>>> GetCalendario()
        {
            // Reutilizamos la lógica del servicio de estadísticas
            var calendario = await _estadisticasService.ObtenerCalendarioAsync();
            return Ok(calendario);
        }

        // GET: api/Partidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partido>> GetPartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);

            if (partido == null)
            {
                return NotFound();
            }

            return partido;
        }

        // PUT: api/Partidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartido(int id, Partido partido)
        {
            if (id != partido.Id)
            {
                return BadRequest();
            }

            // Validación: Verificar que las selecciones no estén eliminadas
            var local = await _context.Selecciones.FindAsync(partido.LocalId);
            var visitante = await _context.Selecciones.FindAsync(partido.VisitanteId);

            if (local != null && local.Eliminada)
                return BadRequest(new { mensaje = $"La selección {local.Nombre} está eliminada del torneo." });
            if (visitante != null && visitante.Eliminada)
                return BadRequest(new { mensaje = $"La selección {visitante.Nombre} está eliminada del torneo." });

            try
            {
                _validacionService.ValidarIdentidad(partido.LocalId, partido.VisitanteId);
                await _validacionService.ValidarLímitePartidosFaseAsync(partido.FaseCodigo, partido.Id);

                if (partido.FaseCodigo == "GRUPOS")
                {
                    await _validacionService.ValidarMaximoPartidosGrupoAsync(partido.LocalId, partido.Id);
                    await _validacionService.ValidarMaximoPartidosGrupoAsync(partido.VisitanteId, partido.Id);
                }
                else
                {
                    await _validacionService.ValidarUnicidadFaseEliminatoriaAsync(partido.FaseCodigo, partido.LocalId, partido.Id);
                    await _validacionService.ValidarUnicidadFaseEliminatoriaAsync(partido.FaseCodigo, partido.VisitanteId, partido.Id);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            _context.Entry(partido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartidoExists(id))
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

        // POST: api/Partidos
        [HttpPost]
        public async Task<ActionResult<Partido>> PostPartido(Partido partido)
        {
            // Validación: Verificar que las selecciones no estén eliminadas
            var local = await _context.Selecciones.FindAsync(partido.LocalId);
            var visitante = await _context.Selecciones.FindAsync(partido.VisitanteId);

            if (local != null && local.Eliminada)
                return BadRequest(new { mensaje = $"La selección {local.Nombre} está eliminada del torneo." });
            if (visitante != null && visitante.Eliminada)
                return BadRequest(new { mensaje = $"La selección {visitante.Nombre} está eliminada del torneo." });

            try
            {
                _validacionService.ValidarIdentidad(partido.LocalId, partido.VisitanteId);
                await _validacionService.ValidarLímitePartidosFaseAsync(partido.FaseCodigo);

                if (partido.FaseCodigo == "GRUPOS")
                {
                    await _validacionService.ValidarMaximoPartidosGrupoAsync(partido.LocalId);
                    await _validacionService.ValidarMaximoPartidosGrupoAsync(partido.VisitanteId);
                }
                else
                {
                    await _validacionService.ValidarUnicidadFaseEliminatoriaAsync(partido.FaseCodigo, partido.LocalId);
                    await _validacionService.ValidarUnicidadFaseEliminatoriaAsync(partido.FaseCodigo, partido.VisitanteId);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            _context.Partidos.Add(partido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartido", new { id = partido.Id }, partido);
        }

        // DELETE: api/Partidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null)
            {
                return NotFound();
            }

            _context.Partidos.Remove(partido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // RF11 — Registrar resultado oficial de un partido
        // RF12 — Notificación automática al servicio UTNGolCoin

        // Registra el marcador oficial de un partido. Crea automáticamente un registro de auditoría y notifica al servicio UTNGolCoin.

        // PUT: api/Partidos/5/resultado
        [HttpPut("{id}/resultado")]
        public async Task<IActionResult> RegistrarResultado(int id, [FromBody] RegistrarResultadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var partido = await _resultadoService.RegistrarResultadoAsync(id, dto);

                if (partido == null)
                    return NotFound(new { mensaje = $"No se encontró el partido con Id {id}." });

                return Ok(new
                {
                    mensaje = "Resultado registrado exitosamente.",
                    partido = new
                    {
                        partido.Id,
                        partido.NumeroPartidoFifa,
                        partido.GolesLocal,
                        partido.GolesVisitante,
                        partido.Estado
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        private bool PartidoExists(int id)
        {
            return _context.Partidos.Any(e => e.Id == id);
        }
    }
}