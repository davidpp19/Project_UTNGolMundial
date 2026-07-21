using Project_UTNGolMundial.DTOs;

namespace Project_UTNGolMundial.Services
{
    // Cliente HTTP para consumir la API del Servicio UTNGolCoin (RF12).
    public interface IUtnGolCoinClient
    {
        // Envía una notificación HTTP POST al servicio UTNGolCoin con el resultado del partido para que liquide los premios correspondientes.
        Task<bool> NotificarResultadoAsync(NotificacionResultadoDto notificacion);
    }
}
