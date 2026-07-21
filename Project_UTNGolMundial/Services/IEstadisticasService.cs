using Project_UTNGolMundial.DTOs;

namespace Project_UTNGolMundial.Services
{

    // Servicio de consulta pública de estadísticas del torneo.
    public interface IEstadisticasService
    {
        // Obtiene el calendario completo de partidos ordenado por fecha (RF04).
        Task<List<CalendarioPartidoDto>> ObtenerCalendarioAsync();

        // Obtiene las tablas de posiciones calculadas para cada grupo (RF05).
        Task<List<TablaPosicionGrupoDto>> ObtenerTablasPosicionesAsync();

        // Obtiene las estadísticas acumuladas de una selección específica (RF07).
        Task<EstadisticaSeleccionDto?> ObtenerEstadisticaSeleccionAsync(int seleccionId);
    }
}
