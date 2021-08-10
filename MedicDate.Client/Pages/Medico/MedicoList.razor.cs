using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public class MedicoListBase : BaseListComponent<MedicoResponse>
    {
        [Inject] public DialogService DialogService { get; set; }

        private const string GetUrl = "api/Medico/listarConPaginacion?traerEspecialidades=true";
        protected List<EspecialidadResponse> Especialidades = new();
        protected string[] PropNames = { "Nombre", "Apellidos", "Cedula", "PhoneNumber" };
        protected string[] Headers = { "Nombre", "Apellidos", "Cédula", "Teléfono" };

        protected readonly OpRoutes OpRoutes = new()
        { AddUrl = "medicoCrear", EditUrl = "medicoEditar", GetUrl = GetUrl };

        private readonly RenderFragment<object> _especialidadesDialogContent = obj => __builder =>
        {
            var especialidad = (EspecialidadResponse)obj;

            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "mx-3 my-2");
            __builder.AddContent(3, especialidad.NombreEspecialidad);
            __builder.CloseElement();
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemListAsync(GetUrl);

            var httpResponse = await HttpRepo.Get<List<EspecialidadResponse>>("api/Especialidad/listar");

            if (!httpResponse.Error)
            {
                Especialidades = httpResponse.Response;
            }
        }

        protected async Task DeleteMedico(string id)
        {
            await DeleteItem(id, "api/Medico/eliminar", GetUrl);
        }

        protected async Task FilterByEspecialidad(object value)
        {
            try
            {
                var especialidadId = value?.ToString() ?? "";

                await LoadItemListAsync(GetUrl, "&filtrarEspecialidadId=",
                    especialidadId);
            }
            catch (Exception)
            {
                NotificationService.ShowError("Error!", "Error al obtener el Id de la especialidad");
            }
        }

        protected async Task OpenEspecialidadesDialog()
        {
            await DialogService.OpenAsync<RadzenGenericDialog>
            ("",
                new Dictionary<string, object>
                {
                    { "Heading", "Especialidades" },
                    { "ItemList", Especialidades.ToArray() },
                    { "ListBodyContent", _especialidadesDialogContent}
                });
        }
    }
}