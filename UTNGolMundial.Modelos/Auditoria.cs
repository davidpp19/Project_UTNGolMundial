using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Auditoria
    {
        [Key]
        public long Id { get; set; }
        public string TablaAfectada { get; set; }
        public int RegistroId { get; set; }
        public string TipoAccionAuditoria { get; set; }
        public string DatosAnteriores { get; set; }
        public string DatosNuevos { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; } //Clave foranea para relacionar con Usuario
        public Usuario? Usuario { get; set; } //Relacion muchos a uno con Usuario, muchas acciones de auditoria pueden ser realizadas por un usuario
    }
}
