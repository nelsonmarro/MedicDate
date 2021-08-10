using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs.Especialidad;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Especialidad
{
    public class EspecialidadListBase : BaseListComponent<EspecialidadResponse>
    {
        protected readonly string[] TableHeaders = { "Nombre" };
        protected readonly string[] PropNames = { "NombreEspecialidad" };
        protected const string GetUrl = "api/Especialidad/listarConPaginacion";

        protected readonly OpRoutes OpRoutes = new()
        { AddUrl = "especialidadCrear", EditUrl = "especialidadEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemListAsync(GetUrl);
        }

        protected async Task DeleteEspecialidad(string id)
        {
            await DeleteItem(id, "api/Especialidad/eliminar", GetUrl);
        }
    }
}