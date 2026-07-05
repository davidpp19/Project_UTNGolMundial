using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Data
{
    public class MiApiUTNGolMundialContext : DbContext
    {
        public MiApiUTNGolMundialContext(DbContextOptions<MiApiUTNGolMundialContext> options)
            : base(options)
        {
        }

        public DbSet<Fase> Fases { get; set; } = default!;
        public DbSet<Grupo> Grupos { get; set; } = default!;
        public DbSet<Partido> Partidos { get; set; } = default!;
        public DbSet<Sede> Sedes { get; set; } = default!;
        public DbSet<Seleccion> Selecciones { get; set; } = default!;
    }
}