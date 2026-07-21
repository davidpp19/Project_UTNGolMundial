using Microsoft.AspNetCore.Mvc;
using Project_UTNGolMundial.DTOs;
using Project_UTNGolMundial.Services;

namespace Project_UTNGolMundial.Controllers
{
    /// <summary>
    /// Controlador de consulta pública de estadísticas del torneo.
    /// Expone endpoints de solo lectura para el Frontend Público.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController : ControllerBase
    {
        private readonly IEstadisticasService _estadisticasService;

        public EstadisticasController(IEstadisticasService estadisticasService)
        {
            _estadisticasService = estadisticasService;
        }

        /// <summary>
        /// RF04 — Obtiene el calendario completo de partidos del torneo,
        /// ordenado cronológicamente, con información de fase, grupo, sede y selecciones.
        /// </summary>
        /// <returns>Lista de partidos con toda la información aplanada en DTOs.</returns>
        // GET: api/estadisticas/calendario
        [HttpGet("calendario")]
        public async Task<ActionResult<List<CalendarioPartidoDto>>> GetCalendario()
        {
            var calendario = await _estadisticasService.ObtenerCalendarioAsync();
            return Ok(calendario);
        }

        /// <summary>
        /// RF05 — Obtiene las tablas de posiciones calculadas para cada grupo.
        /// Cada grupo incluye sus selecciones ordenadas por: Puntos desc → DG desc → GF desc.
        /// </summary>
        /// <returns>Lista de grupos con sus tablas de posiciones.</returns>
        // GET: api/estadisticas/posiciones
        [HttpGet("posiciones")]
        public async Task<ActionResult<List<TablaPosicionGrupoDto>>> GetPosiciones()
        {
            var posiciones = await _estadisticasService.ObtenerTablasPosicionesAsync();
            return Ok(posiciones);
        }

        /// <summary>
        /// RF07 — Obtiene las estadísticas acumuladas de una selección específica:
        /// partidos jugados, ganados, empatados, perdidos, goles a favor y en contra.
        /// Incluye partidos de todas las fases del torneo (grupos + eliminatorias).
        /// </summary>
        /// <param name="id">ID de la selección.</param>
        /// <returns>Estadísticas de la selección o 404 si no existe.</returns>
        // GET: api/estadisticas/selecciones/5
        [HttpGet("selecciones/{id}")]
        public async Task<ActionResult<EstadisticaSeleccionDto>> GetEstadisticaSeleccion(int id)
        {
            var estadistica = await _estadisticasService.ObtenerEstadisticaSeleccionAsync(id);

            if (estadistica == null)
                return NotFound(new { mensaje = $"No se encontró la selección con Id {id}." });

            return Ok(estadistica);
        }
    }
}
