namespace UTNGolMundial.Consumer
{
    // Contrato del cliente HTTP para el Servicio UTNGolCoin (RF12). Define el método que notifica el resultado de un partido para que el servicio externo liquide los premios.
    public interface IUtnGolCoinClient
    {
        // Envía una petición HTTP POST al endpoint de liquidación del Servicio UTNGolCoin, notificando el PartidoId y el resultado ganador.
        Task<bool> NotificarResultadoAsync(NotificacionResultadoDto notificacion);
    }
}
