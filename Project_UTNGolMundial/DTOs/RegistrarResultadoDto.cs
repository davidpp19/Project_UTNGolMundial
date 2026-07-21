using System.ComponentModel.DataAnnotations;

namespace Project_UTNGolMundial.DTOs
{
    // DTO de entrada para registrar el resultado oficial de un partido (RF11). El UsuarioId identifica al administrador que registra el resultado para la auditoría.
    public class RegistrarResultadoDto
    {
        [Required(ErrorMessage = "Los goles del equipo local son obligatorios.")]
        [Range(0, 99, ErrorMessage = "Los goles deben ser un valor entre 0 y 99.")]
        public int GolesLocal { get; set; }

        [Required(ErrorMessage = "Los goles del equipo visitante son obligatorios.")]
        [Range(0, 99, ErrorMessage = "Los goles deben ser un valor entre 0 y 99.")]
        public int GolesVisitante { get; set; }

        [Required(ErrorMessage = "El UsuarioId del administrador es obligatorio para la auditoría.")]
        public int UsuarioId { get; set; }
    }
}
