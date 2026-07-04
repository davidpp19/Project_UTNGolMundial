using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Modelos
{
    public class Partido
    {
        [Key]
        public int Id { get; set; }
        public int NumeroPartidoFifa { get; set; }
        public DateTime FechaPartido { get; set; }
        public Boolean Estado { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }

        [ForeignKey("FaseCodigo")]
        public int FaseCodigo { get; set; } //Clave foranea para relacionar con Fase

        [ForeignKey("GrupoCodigo")]
        public int GrupoCodigo { get; set; } //Clave foranea para relacionar con Grupo

        [ForeignKey("SedeId")]
        public int SedeId { get; set; } //Clave foranea para relacionar con Sede

        [ForeignKey("SeleccionLocal")]
        public int SeleccionLocal { get; set; } //Clave foranea para relacionar con Seleccion

        [ForeignKey("SeleccionVisitante")]
        public int SeleccionVisitante { get; set; } //Clave foranea para relacionar con Seleccion

        //Relaciones
        public Fase? Fase { get; set; } //Relacion muchos a uno con Fase, muchos partidos pueden pertenecer a una fase, objeto de navegacion.

        
        public Grupo? Grupo { get; set; } //Relacion muchos a uno con Grupo, muchos partidos pueden pertenecer a un grupo, objeto de navegacion.
        
        public Sede? Sede { get; set; } //Relacion muchos a uno con Sede, muchos partidos pueden pertenecer a una sede, objeto de navegacion.
        
        public Seleccion? Local { get; set; } //Relacion muchos a uno con Seleccion, muchos partidos pueden tener una seleccion local, objeto de navegacion.
        
        public Seleccion? Visitante { get; set; } //Relacion muchos a uno con Seleccion, muchos partidos pueden tener una seleccion visitante, objeto de navegacion.
    }
}
