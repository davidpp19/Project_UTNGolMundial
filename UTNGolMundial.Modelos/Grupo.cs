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
        public char Codigo { get; set; }
        public string Nombre { get; set; }

        //Relacion 
        public List<Seleccion>? Selecciones { get; set; } //Relacion uno a muchos con Seleccion, un grupo puede tener muchas selecciones
        public List<Partido>? Partidos { get; set; } //Relacion uno a muchos con Partido, un grupo puede tener muchos partidos
    }
}
