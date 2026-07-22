namespace UTNGolMundial.Consumer
{
    // DTO con el payload JSON que se envía al Servicio UTNGolCoin (RF12) para notificar el resultado de un partido y liquidar los premios correspondientes.
    public class NotificacionResultadoDto
    {
        // ID del partido finalizado
        public int PartidoId { get; set; }

        // Resultado final del partido
        public string ResultadoFinal { get; set; } = string.Empty;
    }
}
