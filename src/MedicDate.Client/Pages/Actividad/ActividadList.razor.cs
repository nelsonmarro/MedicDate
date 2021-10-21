using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Actividad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Actividad
{
    public partial class ActividadList
    {
        [Inject]
        public IBaseListComponentOperations BaseListComponentOps { get; set; } = default!;

        private IEnumerable<ActividadResponseDto>? _actividadList;
        private int _totalCount = 0;
        private readonly string[] _tableHeaders = { "Nombre" };
        private readonly string[] _propNames = { "Nombre" };
        private const string GetUrl = "api/Actividad/listarConPaginacion";

        private readonly OpRoutes _opRoutes = new()
        { AddUrl = "actividadCrear", EditUrl = "actividadEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            var result = await BaseListComponentOps.LoadItemListAsync<ActividadResponseDto>(GetUrl);

            if (result.Succeded)
            {
                _actividadList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }

        private async Task DeleteActividad(string idString)
        {
            const string deleteUrl = "api/Actividad/eliminar";
            var result = await BaseListComponentOps.DeleteItem<ActividadResponseDto>(idString,
                deleteUrl, GetUrl);

            if (result.Succeded)
            {
                _actividadList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }
    }
}
