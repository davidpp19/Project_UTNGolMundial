namespace Project_UTNGolMundial.DTOs
{
    // DTO que agrupa la tabla de posiciones de un grupo específico (RF05).
    public class TablaPosicionGrupoDto
    {
        public char GrupoCodigo { get; set; }
        public string GrupoNombre { get; set; } = string.Empty;
        public List<PosicionSeleccionDto> Posiciones { get; set; } = new();
    }

    // DTO con las estadísticas de una selección dentro de su grupo. Ordenamiento: Pts desc : DG desc : GF desc.
    public class PosicionSeleccionDto
    {
        public int SeleccionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoFifa { get; set; } = string.Empty;

        // Partidos Jugados
        public int PJ { get; set; }

        // Partidos Ganados
        public int PG { get; set; }

        // Partidos Empatados
        public int PE { get; set; }

        // Partidos Perdidos
        public int PP { get; set; }

        // Goles a Favor
        public int GF { get; set; }

        // Goles en Contra
        public int GC { get; set; }

        // Diferencia de Goles (GF - GC)
        public int DG { get; set; }

        // Puntos (PG*3 + PE*1)
        public int Pts { get; set; }
    }
}
