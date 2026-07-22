namespace UTNGolMundial.Consumer
{
    // Contrato del cliente HTTP para el Servicio UTNGolCoin (RF12). Define el método que notifica el resultado de un partido para que el servicio externo liquide los premios.
    public interface IUtnGolCoinClient
    {
        // Envía una petición HTTP POST al endpoint de liquidación del Servicio UTNGolCoin
        Task<bool> NotificarResultadoAsync(NotificacionResultadoDto notificacion);

        // Notifica el registro de un nuevo usuario
        Task NotificarRegistroAsync(int id, string username, string nombre, bool activo);
    }
}
