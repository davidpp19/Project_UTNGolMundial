namespace Project_UTNGolMundial.DTOs
{
    // DTO con el payload JSON que se envía al Servicio UTNGolCoin (RF12) para notificar el resultado de un partido y liquidar premios.
    public class NotificacionResultadoDto
    {
        public int PartidoId { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }

        // Resultado del partido: "Local", "Visitante" o "Empate".
        public string Resultado { get; set; } = string.Empty;
    }
}
