using System.Threading.Tasks;
using UTNGolMundial.Modelos;

namespace Project_UTNGolMundial.Services
{
    public interface ITorneoValidacionService
    {
        void ValidarIdentidad(int localId, int visitanteId);
        Task ValidarLímitesGrupoAsync(int seleccionId, char grupoCodigo);
        Task ValidarMaximoPartidosGrupoAsync(int seleccionId, int? excluyendoPartidoId = null);
        Task ValidarUnicidadFaseEliminatoriaAsync(string faseCodigo, int seleccionId, int? excluyendoPartidoId = null);
    }
}
