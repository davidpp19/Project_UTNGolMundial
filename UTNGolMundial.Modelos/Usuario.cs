using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        [ForeignKey("RolId")]
        public short RolId { get; set; } //Clave foranea para relacionar con Rol
        public Rol? Rol { get; set; } //Relacion muchos a uno con Rol, muchos usuarios pueden tener un mismo rol
        public List<Auditoria>? Auditorias { get; set; } //Relacion uno a muchos con Auditoria, un usuario puede registrar muchas auditorias

    }
}
