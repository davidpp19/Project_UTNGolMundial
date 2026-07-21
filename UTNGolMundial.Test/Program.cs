using UTNGolMundial.Consumer;
using UTNGolMundial.Modelos;

namespace UTNGolMundial.Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Configuramos los endpoints usando la base de la API
            Crud<Rol>.Endpoint = "https://localhost:7053/api/Roles";
            Crud<Sede>.Endpoint = "https://localhost:7053/api/Sedes";

            Console.WriteLine("Endpoint de Roles: " + Crud<Rol>.Endpoint);
            Console.WriteLine("Endpoint de Sedes: " + Crud<Sede>.Endpoint);

            try
            {
                // Ejemplo: Crear un nuevo Rol (comentado para evitar llenar la BD)
                /*
                var nuevoRol = await Crud<Rol>.Create(new Rol 
                { 
                    Id = 99, 
                    Nombre = "Auditor", 
                    Descripcion = "Rol de prueba", 
                    Permisos = "SoloLectura" 
                });
                */

                // Ejemplo: Crear una nueva Sede (comentado)
                /*
                var nuevaSede = await Crud<Sede>.Create(new Sede
                {
                    Nombre = "Estadio de Prueba",
                    Ciudad = "Ciudad Test",
                    Pais = "Pais Test",
                    CapacidadAproximada = 50000
                });
                */

                // Ejemplo: Eliminar (comentado)
                // Crud<Rol>.Delete("99");

                // Actualizar un Rol (Asegúrate de que el Id=2 exista en tu BD)
                Crud<Rol>.Update("2", new Rol 
                { 
                    Id = 2, 
                    Nombre = "Actualizado", 
                    Descripcion = "Rol actualizado desde consola de Test", 
                    Permisos = "Modificado" 
                });

                Console.WriteLine("Registro actualizado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
