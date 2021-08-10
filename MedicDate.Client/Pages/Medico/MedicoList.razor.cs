using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Models.DTOs.Especialidad;
using Radzen;

namespace MedicDate.Client.Pages.Medico
{
    public class MedicoListBase : BaseListComponent<MedicoResponse>, IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        protected static string GetUrl = "api/Medico/listarConPaginacion?traerEspecialidades=true";
        protected List<EspecialidadResponse> Especialidades = new();
        protected string[] PropNames = { "Nombre", "Apellidos", "Cedula", "PhoneNumber"};
        protected string[] Headers = { "Nombre", "Apellidos", "Cédula", "Teléfono"};

        protected readonly OpRoutes OpRoutes = new()
            {AddUrl = "medicoCrear", EditUrl = "medicoEditar", GetUrl = GetUrl};

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
            HttpInterceptor.RegisterEvent();

            await LoadItemListAsync(GetUrl);

            var httpResponse = await HttpRepo.Get<List<EspecialidadResponse>>("api/Especialidad/listar");

            if (httpResponse is null)
            {
                return;
            }

            if (httpResponse.Error)
            {
                NotificationService.ShowError("Error!", await httpResponse.GetResponseBody());
            }
            else
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

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}