using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Sede
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public int CapacidadAproximada { get; set; }

        //Relacion
        public List<Partido>? Partidos { get; set; } //Relacion uno a muchos con Partido, una sede puede tener muchos partidos

    }
}
