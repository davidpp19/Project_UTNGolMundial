using Project_UTNGolMundial.Data;
using Project_UTNGolMundial.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UTNGolMundial.Consumer;

namespace MiApi.UTNGolMundial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MiApiUTNGolMundialContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL") ??
                throw new InvalidOperationException("Connection string 'PostgreSQL' not found.")));

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Servicios de negocio (Estadísticas y Registro de Resultados) 
            builder.Services.AddScoped<IEstadisticasService, EstadisticasService>();
            builder.Services.AddScoped<IPartidoResultadoService, PartidoResultadoService>();

            // HTTP Client — Servicio UTNGolCoin (RF12) 
            // Registra el Typed Client del proyecto UTNGolMundial.Consumer.
            // La URL base se configura en appsettings.json : ServiciosExternos:UTNGolCoinUrl
            // IUtnGolCoinClient y UtnGolCoinClient provienen de UTNGolMundial.Consumer.
            builder.Services.AddHttpClient<IUtnGolCoinClient, UtnGolCoinClient>(client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ServiciosExternos:UTNGolCoinUrl"]
                    ?? "https://localhost:7053");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi(); //Esta línea se utiliza para mapear la documentación OpenAPI generada por NSwag a una ruta específica en la aplicación. Sin embargo, en este caso, no es necesaria porque el middleware de Swagger ya se encarga de exponer la documentación en la ruta predeterminada (/swagger).

                app.UseSwagger();
                app.UseSwaggerUI(); //La interfaz de usuario de Swagger se habilita solo en desarrollo, por seguridad. En producción, es recomendable deshabilitarla para evitar exponer información sensible sobre la API.
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }  
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
