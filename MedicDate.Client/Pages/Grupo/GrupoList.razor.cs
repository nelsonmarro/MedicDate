using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs.Grupo;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Grupo
{
    public class GrupoListBase : BaseListComponent<GrupoResponse>
    {
        protected readonly string[] TableHeaders = { "Nombre" };
        protected readonly string[] PropNames = { "Nombre" };
        protected const string GetUrl = "api/Grupo/listarConPaginacion";

        protected readonly OpRoutes OpRoutes = new()
        { AddUrl = "grupoCrear", EditUrl = "grupoEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemListAsync(GetUrl);
        }

        protected async Task DeleteGrupo(string idString)
        {
            const string deleteUrl = "api/Grupo/eliminar";
            await DeleteItem(idString, deleteUrl, GetUrl);
        }
    }
}
