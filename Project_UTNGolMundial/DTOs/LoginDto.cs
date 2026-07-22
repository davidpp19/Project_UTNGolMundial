using System.ComponentModel.DataAnnotations;

namespace Project_UTNGolMundial.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario o correo es obligatorio.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = string.Empty;
    }
}
