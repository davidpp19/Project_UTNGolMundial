namespace UTNGolMundial.Consumer
{
    // DTO con el payload JSON que se envía al Servicio UTNGolCoin (RF12) para notificar el resultado de un partido y liquidar los premios correspondientes.
    public class NotificacionResultadoDto
    {
        // ID del partido finalizado
        public int PartidoId { get; set; }

        // Goles marcados por el equipo local.
        public int GolesLocal { get; set; }

        // Goles marcados por el equipo visitante.
        public int GolesVisitante { get; set; }

        // Resultado del partido: "Local", "Visitante" o "Empate"
        public string Resultado { get; set; } = string.Empty;
    }
}
