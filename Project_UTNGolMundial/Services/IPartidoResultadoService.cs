using Project_UTNGolMundial.DTOs;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Services
{
    // Servicio para registrar el resultado oficial de un partido (RF11). Incluye auditoría automática y notificación al servicio UTNGolCoin (RF12).
    public interface IPartidoResultadoService
    {
        // Registra el marcador oficial de un partido, crea el registro de auditoría, y notifica al servicio UTNGolCoin para la liquidación de premios.
        Task<Partido?> RegistrarResultadoAsync(int partidoId, RegistrarResultadoDto dto);
    }
}
