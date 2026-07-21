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
        public string Estado { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }

        [ForeignKey("FaseCodigo")]
        public string FaseCodigo { get; set; } //Clave foranea para relacionar con Fase

        [ForeignKey("GrupoCodigo")]
        public char GrupoCodigo { get; set; } //Clave foranea para relacionar con Grupo

        [ForeignKey("SedeId")]
        public int SedeId { get; set; } //Clave foranea para relacionar con Sede

        [ForeignKey("LocalId")]
        public int LocalId { get; set; } //Clave foranea para relacionar con Seleccion

        [ForeignKey("VisitanteId")]
        public int VisitanteId { get; set; } //Clave foranea para relacionar con Seleccion

        //Relaciones y objetos de navegacion
        public Fase? Fase { get; set; } //Relacion muchos a uno con Fase, muchos partidos pueden pertenecer a una fase
        public Grupo? Grupo { get; set; } //Relacion muchos a uno con Grupo, muchos partidos pueden pertenecer a un grupo
        public Sede? Sede { get; set; } //Relacion muchos a uno con Sede, muchos partidos pueden pertenecer a una sede
        public Seleccion? Local { get; set; } //Relacion muchos a uno con Seleccion, muchos partidos pueden tener una seleccion local
        public Seleccion? Visitante { get; set; } //Relacion muchos a uno con Seleccion, muchos partidos pueden tener una seleccion visitante
    }
}
