using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;
using Project_UTNGolMundial.DTOs;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Services
{
    // Implementación del servicio de estadísticas públicas. Consulta la bdd y calcula tablas de posiciones y estadísticas por selección.
    public class EstadisticasService : IEstadisticasService
    {
        private readonly MiApiUTNGolMundialContext _context;

        public EstadisticasService(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        // RF04 — Calendario completo de partidos
        public async Task<List<CalendarioPartidoDto>> ObtenerCalendarioAsync()
        {
            var partidos = await _context.Partidos
                .Include(p => p.Fase)
                .Include(p => p.Grupo)
                .Include(p => p.Sede)
                .Include(p => p.Local)
                .Include(p => p.Visitante)
                .OrderBy(p => p.FechaPartido)
                .ThenBy(p => p.NumeroPartidoFifa)
                .AsNoTracking()
                .ToListAsync();

            return partidos.Select(p => new CalendarioPartidoDto
            {
                PartidoId = p.Id,
                NumeroPartidoFifa = p.NumeroPartidoFifa,
                FechaPartido = p.FechaPartido,
                Estado = p.Estado,

                FaseCodigo = p.FaseCodigo,
                FaseNombre = p.Fase?.Nombre ?? string.Empty,

                GrupoCodigo = p.Grupo != null ? (char?)p.GrupoCodigo : null,
                GrupoNombre = p.Grupo?.Nombre,

                SedeId = p.SedeId,
                SedeNombre = p.Sede?.Nombre ?? string.Empty,
                SedeCiudad = p.Sede?.Ciudad ?? string.Empty,
                SedePais = p.Sede?.Pais ?? string.Empty,

                LocalId = p.LocalId,
                LocalNombre = p.Local?.Nombre ?? string.Empty,
                LocalCodigoFifa = p.Local?.CodigoFifa ?? string.Empty,

                VisitanteId = p.VisitanteId,
                VisitanteNombre = p.Visitante?.Nombre ?? string.Empty,
                VisitanteCodigoFifa = p.Visitante?.CodigoFifa ?? string.Empty,

                GolesLocal = p.GolesLocal,
                GolesVisitante = p.GolesVisitante
            }).ToList();
        }

        // RF05 : Tablas de posiciones por grupo
        public async Task<List<TablaPosicionGrupoDto>> ObtenerTablasPosicionesAsync()
        {
            // Traer todos los grupos con sus selecciones
            var grupos = await _context.Grupos
                .Include(g => g.Selecciones)
                .AsNoTracking()
                .OrderBy(g => g.Codigo)
                .ToListAsync();

            // Traer todos los partidos finalizados de fase de grupos
            var partidosFinalizados = await _context.Partidos
                .Where(p => p.Estado == "Finalizado")
                .AsNoTracking()
                .ToListAsync();

            var resultado = new List<TablaPosicionGrupoDto>();

            foreach (var grupo in grupos)
            {
                // Filtrar partidos de este grupo
                var partidosDelGrupo = partidosFinalizados
                    .Where(p => p.GrupoCodigo == grupo.Codigo)
                    .ToList();

                var posiciones = new List<PosicionSeleccionDto>();

                if (grupo.Selecciones != null)
                {
                    foreach (var seleccion in grupo.Selecciones)
                    {
                        var stats = CalcularEstadisticas(seleccion.Id, partidosDelGrupo);
                        posiciones.Add(stats);
                    }
                }

                // Ordenar: Puntos desc : Diferencia de Goles desc : Goles a Favor desc
                posiciones = posiciones
                    .OrderByDescending(p => p.Pts)
                    .ThenByDescending(p => p.DG)
                    .ThenByDescending(p => p.GF)
                    .ToList();

                resultado.Add(new TablaPosicionGrupoDto
                {
                    GrupoCodigo = grupo.Codigo,
                    GrupoNombre = grupo.Nombre,
                    Posiciones = posiciones
                });
            }

            return resultado;
        }

        // RF07 : Estadísticas por selección
        public async Task<EstadisticaSeleccionDto?> ObtenerEstadisticaSeleccionAsync(int seleccionId)
        {
            var seleccion = await _context.Selecciones
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == seleccionId);

            if (seleccion == null)
                return null;

            // Obtener TODOS los partidos finalizados de esta selección (grupos + eliminatorias)
            var partidos = await _context.Partidos
                .Where(p => p.Estado == "Finalizado"
                    && (p.LocalId == seleccionId || p.VisitanteId == seleccionId))
                .AsNoTracking()
                .ToListAsync();

            int ganados = 0, empatados = 0, perdidos = 0, gf = 0, gc = 0;

            foreach (var p in partidos)
            {
                if (p.LocalId == seleccionId)
                {
                    gf += p.GolesLocal;
                    gc += p.GolesVisitante;

                    if (p.GolesLocal > p.GolesVisitante) ganados++;
                    else if (p.GolesLocal == p.GolesVisitante) empatados++;
                    else perdidos++;
                }
                else // Visitante
                {
                    gf += p.GolesVisitante;
                    gc += p.GolesLocal;

                    if (p.GolesVisitante > p.GolesLocal) ganados++;
                    else if (p.GolesVisitante == p.GolesLocal) empatados++;
                    else perdidos++;
                }
            }

            return new EstadisticaSeleccionDto
            {
                SeleccionId = seleccion.Id,
                Nombre = seleccion.Nombre,
                CodigoFifa = seleccion.CodigoFifa,
                Confederacion = seleccion.Confederacion,
                PartidosJugados = partidos.Count,
                Ganados = ganados,
                Empatados = empatados,
                Perdidos = perdidos,
                GolesAFavor = gf,
                GolesEnContra = gc,
                DiferenciaDeGoles = gf - gc
            };
        }

        // Método auxiliar: calcula PJ/PG/PE/PP/GF/GC/DG/Pts
        // para una selección dentro de una lista de partidos.
        private PosicionSeleccionDto CalcularEstadisticas(int seleccionId, List<Partido> partidos)
        {
            var seleccion = _context.Selecciones
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == seleccionId);

            int pj = 0, pg = 0, pe = 0, pp = 0, gf = 0, gc = 0;

            foreach (var p in partidos)
            {
                bool esLocal = p.LocalId == seleccionId;
                bool esVisitante = p.VisitanteId == seleccionId;

                if (!esLocal && !esVisitante) continue;

                pj++;

                if (esLocal)
                {
                    gf += p.GolesLocal;
                    gc += p.GolesVisitante;

                    if (p.GolesLocal > p.GolesVisitante) pg++;
                    else if (p.GolesLocal == p.GolesVisitante) pe++;
                    else pp++;
                }
                else
                {
                    gf += p.GolesVisitante;
                    gc += p.GolesLocal;

                    if (p.GolesVisitante > p.GolesLocal) pg++;
                    else if (p.GolesVisitante == p.GolesLocal) pe++;
                    else pp++;
                }
            }

            return new PosicionSeleccionDto
            {
                SeleccionId = seleccionId,
                Nombre = seleccion?.Nombre ?? string.Empty,
                CodigoFifa = seleccion?.CodigoFifa ?? string.Empty,
                PJ = pj,
                PG = pg,
                PE = pe,
                PP = pp,
                GF = gf,
                GC = gc,
                DG = gf - gc,
                Pts = (pg * 3) + (pe * 1)
            };
        }
    }
}
