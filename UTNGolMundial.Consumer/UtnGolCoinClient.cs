using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UTNGolMundial.Consumer
{
    // Implementación del cliente HTTP para el Servicio UTNGolCoin (RF12). Este cliente vive en el proyecto UTNGolMundial.Consumer y utiliza el patrón Typed Client de IHttpClientFactory, inyectado por el contenedor DI del proyecto API (Program.cs).
    // Responsabilidad única: enviar POST /api/liquidar con el resultado del partido
    // para que UTNGolCoin liquide los premios de los apostadores ganadores
    public class UtnGolCoinClient : IUtnGolCoinClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UtnGolCoinClient> _logger;

        // Constructor: ASP.NET Core inyecta el HttpClient ya configurado (BaseAddress y headers) desde la registración en Program.cs.
        public UtnGolCoinClient(HttpClient httpClient, ILogger<UtnGolCoinClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> NotificarResultadoAsync(NotificacionResultadoDto notificacion)
        {
            try
            {
                // Serializar con Newtonsoft.Json (consistente con el resto de la solución)
                var json = JsonConvert.SerializeObject(notificacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation(
                    "Enviando notificación para Partido {PartidoId} — Resultado: {Resultado}",
                    notificacion.PartidoId, notificacion.ResultadoFinal);

                // POST al endpoint de liquidación de predicciones
                var response = await _httpClient.PostAsync("/api/predicciones/liquidar", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "[UTNGolCoin] Notificación exitosa para Partido {PartidoId}. HTTP {StatusCode}",
                        notificacion.PartidoId, (int)response.StatusCode);
                    return true;
                }

                // La respuesta llegó pero con error (4xx / 5xx)
                var cuerpoRespuesta = await response.Content.ReadAsStringAsync();
                _logger.LogError(
                    "[UTNGolCoin] Error HTTP {StatusCode} para Partido {PartidoId}. Respuesta: {Body}",
                    (int)response.StatusCode, notificacion.PartidoId, cuerpoRespuesta);
                return false;
            }
            catch (HttpRequestException ex)
            {
                // Error de red: servicio caído, DNS no resuelve, etc.
                _logger.LogError(ex,
                    "[UTNGolCoin] Error de red al notificar Partido {PartidoId}.",
                    notificacion.PartidoId);
                return false;
            }
            catch (TaskCanceledException ex)
            {
                // Timeout: el servicio tardó demasiado en responder
                _logger.LogError(ex,
                    "[UTNGolCoin] Timeout al notificar Partido {PartidoId}.",
                    notificacion.PartidoId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al notificar Partido {PartidoId}.", notificacion.PartidoId);
                return false;
            }
        }

        public async Task NotificarRegistroAsync(int id, string username, string nombre, bool activo)
        {
            try
            {
                // Objeto anónimo para el payload
                var payload = new
                {
                    id = id,
                    username = username,
                    nombre = nombre,
                    activo = activo
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar POST a usuarios registrar
                var response = await _httpClient.PostAsync("/api/usuarios/registrar", content);

                if (!response.IsSuccessStatusCode)
                {
                    var cuerpoRespuesta = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error al notificar registro de usuario {Username}. Respuesta: {Body}", username, cuerpoRespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al notificar registro de usuario {Username}.", username);
            }
        }
    }
}
