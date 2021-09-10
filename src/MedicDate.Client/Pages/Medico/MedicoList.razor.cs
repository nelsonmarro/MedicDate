using MedicDate.API.DTOs.Especialidad;
using MedicDate.API.DTOs.Medico;
using MedicDate.Client.Components;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoList
    {
        [Inject]
        public IBaseListComponentOperations BaseListComponentOps { get; set; }

        [Inject]
        public IHttpRepository HttpRepo { get; set; }

        [Inject] public DialogService DialogService { get; set; }

        private IEnumerable<MedicoResponseDto> _medicoList;
        private int _totalCount = 0;
        private const string GetUrl = "api/Medico/listarConPaginacion?traerEspecialidades=true";
        private List<EspecialidadResponseDto> _especialidadList = new();
        private readonly string[] _propNames = { "Nombre", "Apellidos", "Cedula", "PhoneNumber" };
        private readonly string[] _headers = { "Nombre", "Apellidos", "Cédula", "Teléfono" };

        private readonly OpRoutes _opRoutes = new()
        { AddUrl = "medicoCrear", EditUrl = "medicoEditar", GetUrl = GetUrl };

        private readonly RenderFragment<object> _especialidadesDialogContent = obj => __builder =>
        {
            var especialidad = (EspecialidadResponseDto)obj;

            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "mx-3 my-2");
            __builder.AddContent(3, especialidad.NombreEspecialidad);
            __builder.CloseElement();
        };

        protected override async Task OnInitializedAsync()
        {
            var result = await BaseListComponentOps.LoadItemListAsync<MedicoResponseDto>(GetUrl);

            if (result.Succeded)
            {
                _medicoList = result.ItemList;
                _totalCount = result.TotalCount;
            }

            var httpResponse = await HttpRepo.Get<List<EspecialidadResponseDto>>("api/Especialidad/listar");

            if (!httpResponse.Error)
            {
                _especialidadList = httpResponse.Response;
            }
        }

        private async Task DeleteMedico(string id)
        {
            var result = await BaseListComponentOps.DeleteItem<MedicoResponseDto>(id, "api/Medico/eliminar", GetUrl);

            if (result.Succeded)
            {
                _medicoList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }

        private async Task FilterByEspecialidad(object value)
        {
            var especialidadId = value?.ToString() ?? "";

            var result = await BaseListComponentOps.LoadItemListAsync<MedicoResponseDto>(GetUrl, "&filtrarEspecialidadId=", especialidadId);

            if (result.Succeded)
            {
                _medicoList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }

        private async Task OpenEspecialidadesDialog()
        {
            await DialogService.OpenAsync<RadzenGenericDialog>
            ("",
                new Dictionary<string, object>
                {
                    { "Heading", "Especialidades" },
                    { "ItemList", _especialidadList.ToArray() },
                    { "ListBodyContent", _especialidadesDialogContent }
                });
        }
    }
}