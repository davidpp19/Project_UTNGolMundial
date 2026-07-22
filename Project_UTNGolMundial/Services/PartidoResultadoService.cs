using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project_UTNGolMundial.Data;
using Project_UTNGolMundial.DTOs;
using UTNGolMundial.Consumer;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Services
{
    // Servicio para registrar el resultado oficial de un partido (RF11).
    // Después de guardar el resultado:
    //   1. Crea un registro de auditoría automáticamente.
    //   2. Notifica al servicio UTNGolCoin para liquidar premios (RF12).
    public class PartidoResultadoService : IPartidoResultadoService
    {
        private readonly MiApiUTNGolMundialContext _context;
        private readonly IUtnGolCoinClient _golCoinClient;
        private readonly ILogger<PartidoResultadoService> _logger;

        public PartidoResultadoService(
            MiApiUTNGolMundialContext context,
            IUtnGolCoinClient golCoinClient,
            ILogger<PartidoResultadoService> logger)
        {
            _context = context;
            _golCoinClient = golCoinClient;
            _logger = logger;
        }

        public async Task<Partido?> RegistrarResultadoAsync(int partidoId, RegistrarResultadoDto dto)
        {
            // 1. Buscar el partido
            var partido = await _context.Partidos.FindAsync(partidoId);

            if (partido == null)
            {
                _logger.LogWarning("Partido con Id {PartidoId} no encontrado.", partidoId);
                return null;
            }

            // 2. Validar que no esté ya finalizado
            if (partido.Estado == "FINALIZADO")
            {
                _logger.LogWarning("El partido {PartidoId} ya tiene un resultado registrado.", partidoId);
                throw new InvalidOperationException(
                    $"El partido {partidoId} ya está finalizado. No se puede modificar el resultado.");
            }

            // 3. Capturar datos anteriores para la auditoría
            var datosAnteriores = JsonConvert.SerializeObject(new
            {
                partido.GolesLocal,
                partido.GolesVisitante,
                partido.Estado
            });

            // 4. Registrar el resultado
            partido.GolesLocal = dto.GolesLocal;
            partido.GolesVisitante = dto.GolesVisitante;
            partido.Estado = "FINALIZADO";

            // 5. Capturar datos nuevos para la auditoría
            var datosNuevos = JsonConvert.SerializeObject(new
            {
                partido.GolesLocal,
                partido.GolesVisitante,
                partido.Estado
            });

            // 6. Crear registro de auditoría automáticamente
            var auditoria = new Auditoria
            {
                TablaAfectada = "Partidos",
                RegistroId = partido.Id,
                TipoAccionAuditoria = "RegistroResultado",
                DatosAnteriores = datosAnteriores,
                DatosNuevos = datosNuevos,
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId
            };
            _context.Auditorias.Add(auditoria);

            // 7. Guardar todo en una sola transacción (resultado + auditoría)
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Resultado registrado para partido {PartidoId}: Local {GolesLocal} - Visitante {GolesVisitante}. Auditoría creada.",
                partidoId, dto.GolesLocal, dto.GolesVisitante);

            // 8. Notificar al Servicio UTNGolCoin (RF12) — mediante el proyecto Consumer
            // Se construye el DTO del Consumer (UTNGolMundial.Consumer.NotificacionResultadoDto)
            var notificacion = new UTNGolMundial.Consumer.NotificacionResultadoDto
            {
                PartidoId = partido.Id,
                GolesLocal = partido.GolesLocal.Value,
                GolesVisitante = partido.GolesVisitante.Value,
                Resultado = DeterminarResultado(partido.GolesLocal.Value, partido.GolesVisitante.Value)
            };

            var notificado = await _golCoinClient.NotificarResultadoAsync(notificacion);

            if (!notificado)
            {
                _logger.LogWarning(
                    "No se pudo notificar al servicio UTNGolCoin para el partido {PartidoId}. " +
                    "El resultado fue guardado exitosamente, pero la liquidación de premios queda pendiente.",
                    partidoId);
            }

            return partido;
        }

        // Determina el resultado textual del partido para la notificación.
        private static string DeterminarResultado(int golesLocal, int golesVisitante)
        {
            if (golesLocal > golesVisitante) return "Local";
            if (golesVisitante > golesLocal) return "Visitante";
            return "Empate";
        }
    }
}
