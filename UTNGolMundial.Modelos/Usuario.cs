using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Boolean Activo { get; set; }

    }
}
