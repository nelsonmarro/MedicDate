using MedicDate.API.DTOs.Especialidad;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadList
    {
        [Inject]
        public IBaseListComponentOperations BaseListComponentOps { get; set; }

        private IEnumerable<EspecialidadResponseDto> _especialidadList;
        private int _totalCount = 0;
        private readonly string[] _tableHeaders = { "Nombre" };
        private readonly string[] _propNames = { "NombreEspecialidad" };
        private const string GetUrl = "api/Especialidad/listarConPaginacion";

        private readonly OpRoutes _opRoutes = new()
        { AddUrl = "especialidadCrear", EditUrl = "especialidadEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            var result = await BaseListComponentOps.LoadItemListAsync<EspecialidadResponseDto>(GetUrl);

            if (result.Succeded)
            {
                _especialidadList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }

        private async Task DeleteEspecialidad(string id)
        {
            var result = await BaseListComponentOps.DeleteItem<EspecialidadResponseDto>(id, "api/Especialidad/eliminar", GetUrl);

            if (result.Succeded)
            {
                _especialidadList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }
    }
}