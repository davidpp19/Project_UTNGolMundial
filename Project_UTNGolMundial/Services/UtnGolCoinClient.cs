using System.Text;
using Newtonsoft.Json;
using Project_UTNGolMundial.DTOs;

namespace Project_UTNGolMundial.Services
{
    // API Consumer que utiliza IHttpClientFactory (Typed Client) para enviar notificaciones HTTP POST al Servicio UTNGolCoin (RF12).
    // La URL base se configura en appsettings.json : ServiciosExternos:UTNGolCoinUrl.
    public class UtnGolCoinClient : IUtnGolCoinClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UtnGolCoinClient> _logger;

        public UtnGolCoinClient(HttpClient httpClient, ILogger<UtnGolCoinClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> NotificarResultadoAsync(NotificacionResultadoDto notificacion)
        {
            try
            {
                // Serializar el payload a JSON usando Newtonsoft.Json (consistente con el proyecto)
                var json = JsonConvert.SerializeObject(notificacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation(
                    "Enviando notificación de resultado al servicio UTNGolCoin para partido {PartidoId}...",
                    notificacion.PartidoId);

                // Enviar la petición POST al endpoint de liquidación
                var response = await _httpClient.PostAsync("/api/liquidar", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Notificación enviada exitosamente al servicio UTNGolCoin para partido {PartidoId}. " +
                        "StatusCode: {StatusCode}",
                        notificacion.PartidoId, response.StatusCode);
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError(
                        "Error al notificar al servicio UTNGolCoin para partido {PartidoId}. " +
                        "StatusCode: {StatusCode}, Response: {Response}",
                        notificacion.PartidoId, response.StatusCode, responseBody);
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "Error de conexión al notificar al servicio UTNGolCoin para partido {PartidoId}.",
                    notificacion.PartidoId);
                return false;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex,
                    "Tiempo de espera agotado al notificar al servicio UTNGolCoin para partido {PartidoId}.",
                    notificacion.PartidoId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error inesperado al notificar al servicio UTNGolCoin para partido {PartidoId}.",
                    notificacion.PartidoId);
                return false;
            }
        }
    }
}
