using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_UTNGolMundial.Data;
using UTNGolMundial.Modelos;
using Project_UTNGolMundial.DTOs;
using UTNGolMundial.Consumer;

namespace Project_UTNGolMundial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly MiApiUTNGolMundialContext _context;
        private readonly IUtnGolCoinClient _utnGolCoinClient;

        public UsuariosController(MiApiUTNGolMundialContext context, IUtnGolCoinClient utnGolCoinClient)
        {
            _context = context;
            _utnGolCoinClient = utnGolCoinClient;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            var dtos = usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Username = u.Username,
                Nombre = u.Nombre,
                Email = u.Mail,
                RolNombre = u.Rol?.Nombre.ToUpper() ?? "SIN ROL",
                Activo = u.Activo
            }).ToList();
            
            return Ok(dtos);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Email = usuario.Mail,
                RolNombre = usuario.Rol?.Nombre.ToUpper() ?? "SIN ROL",
                Activo = usuario.Activo
            };
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            // Hashear si la contraseña viene en texto plano (BCrypt usa el prefijo $2)
            if (!string.IsNullOrEmpty(usuario.Password) && !usuario.Password.StartsWith("$2"))
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Hashear contraseña
            if (!string.IsNullOrEmpty(usuario.Password) && !usuario.Password.StartsWith("$2"))
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Notificar registro al microservicio
            await _utnGolCoinClient.NotificarRegistroAsync(usuario.Id, usuario.Username, usuario.Nombre, usuario.Mail, usuario.RolId, usuario.Activo);

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}