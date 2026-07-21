using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Rol
    {
        [Key]
        public short Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Permisos { get; set; }

        //Relaciones y objetos de navegacion
        public List<Usuario>? Usuarios { get; set; } //Relacion uno a muchos con Usuario, un rol puede tener muchos usuarios
    }
}
