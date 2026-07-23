using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;

namespace Project_UTNGolMundial.Services
{
    public class TorneoValidacionService : ITorneoValidacionService
    {
        private readonly MiApiUTNGolMundialContext _context;

        public TorneoValidacionService(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        public void ValidarIdentidad(int localId, int visitanteId)
        {
            if (localId == visitanteId)
            {
                throw new InvalidOperationException("El equipo local y el equipo visitante no pueden ser el mismo.");
            }
        }

        public async Task ValidarLímitesGrupoAsync(int seleccionId, char grupoCodigo)
        {
            // Un equipo solo puede pertenecer a un grupo. Se asume que esto ocurre al actualizar la selección. Contar cuántos equipos hay en el grupo actualmente.
            var cantidad = await _context.Selecciones.CountAsync(s => s.GrupoCodigo == grupoCodigo && s.Id != seleccionId);

            if (cantidad >= 4)
            {
                throw new InvalidOperationException($"El grupo {grupoCodigo} ya tiene 4 equipos. No se puede agregar más.");
            }
        }

        public async Task ValidarMaximoPartidosGrupoAsync(int seleccionId, int? excluyendoPartidoId = null)
        {
            // Cada equipo debe jugar máximo 3 partidos en fase de grupos.
            var query = _context.Partidos.Where(p => 
                p.FaseCodigo == "G" && 
                (p.LocalId == seleccionId || p.VisitanteId == seleccionId));
            
            if (excluyendoPartidoId.HasValue)
            {
                query = query.Where(p => p.Id != excluyendoPartidoId.Value);
            }

            var conteo = await query.CountAsync();

            if (conteo >= 3)
            {
                throw new InvalidOperationException($"La selección con ID {seleccionId} ya tiene programados 3 partidos en la Fase de Grupos.");
            }
        }

        public async Task ValidarUnicidadFaseEliminatoriaAsync(string faseCodigo, int seleccionId, int? excluyendoPartidoId = null)
        {
            // No validar si es fase de grupos
            if (faseCodigo == "G") return;

            var query = _context.Partidos.Where(p => 
                p.FaseCodigo == faseCodigo && 
                (p.LocalId == seleccionId || p.VisitanteId == seleccionId));
            
            if (excluyendoPartidoId.HasValue)
            {
                query = query.Where(p => p.Id != excluyendoPartidoId.Value);
            }

            bool existe = await query.AnyAsync();

            if (existe)
            {
                throw new InvalidOperationException($"La selección con ID {seleccionId} ya está registrada en un partido de la fase {faseCodigo}. No puede jugar dos partidos en la misma fase eliminatoria.");
            }
        }
    }
}
