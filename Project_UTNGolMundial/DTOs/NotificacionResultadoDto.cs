namespace Project_UTNGolMundial.DTOs
{
    // DTO con el payload JSON que se envía al Servicio UTNGolCoin (RF12) para notificar el resultado de un partido y liquidar premios.
    public class NotificacionResultadoDto
    {
        public int PartidoId { get; set; }

        // Resultado final del partido
        public string ResultadoFinal { get; set; } = string.Empty;
    }
}
