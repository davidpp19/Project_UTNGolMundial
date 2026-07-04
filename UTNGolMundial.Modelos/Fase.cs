using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Fase
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int Orden { get; set; }

        //Relacion
        public List<Partido>? Partido { get; set; } //Relacion uno a muchos con Partido, una fase puede tener muchos partidos
    }
}
