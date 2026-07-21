namespace Project_UTNGolMundial.DTOs
{
    // DTO para el calendario completo de partidos (RF04).
    public class CalendarioPartidoDto
    {
        public int PartidoId { get; set; }
        public int NumeroPartidoFifa { get; set; }
        public DateTime FechaPartido { get; set; }
        public string Estado { get; set; } = string.Empty;

        // Fase
        public string FaseCodigo { get; set; } = string.Empty;
        public string FaseNombre { get; set; } = string.Empty;

        // Grupo (acepta nulos por el ?, es como inicializarlo)
        public char? GrupoCodigo { get; set; }
        public string? GrupoNombre { get; set; }

        // Sede
        public int SedeId { get; set; }
        public string SedeNombre { get; set; } = string.Empty;
        public string SedeCiudad { get; set; } = string.Empty;
        public string SedePais { get; set; } = string.Empty;

        // Selección Local
        public int LocalId { get; set; }
        public string LocalNombre { get; set; } = string.Empty;
        public string LocalCodigoFifa { get; set; } = string.Empty;

        // Selección Visitante
        public int VisitanteId { get; set; }
        public string VisitanteNombre { get; set; } = string.Empty;
        public string VisitanteCodigoFifa { get; set; } = string.Empty;

        // Resultado (0 si aún no se jugó; verificar Estado para distinguir)
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
    }
}
