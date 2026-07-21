namespace Project_UTNGolMundial.DTOs
{
    // DTO con las estadísticas completas de una selección en todo el torneo (RF07), Incluye partidos de fase de grupos y eliminatorias.
    public class EstadisticaSeleccionDto
    {
        public int SeleccionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoFifa { get; set; } = string.Empty;
        public string Confederacion { get; set; } = string.Empty;
        public bool EsAnfitrion { get; set; }
        public int PartidosJugados { get; set; }
        public int Ganados { get; set; }
        public int Empatados { get; set; }
        public int Perdidos { get; set; }
        public int GolesAFavor { get; set; }
        public int GolesEnContra { get; set; }
        public int DiferenciaDeGoles { get; set; }
    }
}
