using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Seleccion
    {
        [Key]
        public int Id { get; set; }

        public string CodigoFifa { get; set; }
        public string Nombre { get; set; }
        public string Confederacion { get; set; }
        public Boolean EsAnfitrion { get; set; }
        public string clasificacion { get; set; }

        [ForeignKey("GrupoCodigo")]
        public int GrupoCodigo { get; set; } //Clave foranea para relacionar con Grupo
        public Grupo? Grupo { get; set; } //Relacion muchos a uno con Grupo, muchas selecciones pueden pertenecer a un grupo, objeto de navegacion.

    }
}
