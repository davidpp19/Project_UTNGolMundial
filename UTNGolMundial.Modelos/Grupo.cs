using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Grupo
    {
        [Key]
        public int Codigo { get; set; }
        public char Nombre { get; set; }

        //Relacion 
        public List<Seleccion>? Seleccion { get; set; } //Relacion uno a muchos con Seleccion, un grupo puede tener muchas selecciones
        public List<Partido>? Partido { get; set; } //Relacion uno a muchos con Partido, un grupo puede tener muchos partidos
    }
}
