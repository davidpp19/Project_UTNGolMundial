using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;
using Project_UTNGolMundial.DTOs;

namespace Project_UTNGolMundial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;

        public AuthController(MiApiUTNGolMundialContext context)
        {
            _context = context;
        }

        // POST /api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Buscar usuario por username incluyendo su Rol
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            // Verificar contraseña hasheada
            bool isPasswordValid = false;
            try
            {
                // BCrypt.Verify devuelve true si la contraseña en texto coincide con el hash de la BD
                isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.Password);
            }
            catch (Exception)
            {
                // Si la clave guardada en la base de datos no es un hash válido de BCrypt (ej. texto plano viejo), el Verify lanzará una excepción. La capturamos y rechazamos el login por seguridad.
                isPasswordValid = false;
            }

            if (!isPasswordValid)
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            // Verificar si está activo
            if (!usuario.Activo)
                return StatusCode(403, new { mensaje = "El usuario existe pero está inactivo." });

            // Mapear al DTO para no enviar la contraseña de vuelta y con RolNombre en MAYÚSCULAS
            var responseDto = new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Email = usuario.Mail, // El modelo original usa 'Mail'
                RolNombre = usuario.Rol?.Nombre.ToUpper() ?? "SIN ROL",
                Activo = usuario.Activo
            };

            return Ok(responseDto);
        }
    }
}
