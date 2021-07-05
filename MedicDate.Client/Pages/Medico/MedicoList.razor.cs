using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using Radzen;
using MedicDate.Client.Components;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoList : IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private static string _getUrl = "api/Medico/listarConPaginacion?traerEspecialidades=true";
        private List<MedicoResponse> _medicoList;
        private int _totalCount;
        private List<EspecialidadResponse> _especialidades = new();

        private readonly AllowCrudOps _allowCrudOps = new()
            {AlowAdd = true, AllowEdit = true, AllowDelete = true};

        private readonly OpRoutes _opRoutes = new()
            {AddUrl = "medicoCrear", EditUrl = "medicoEditar", GetUrl = _getUrl};

        private async Task LoadMedicoListAsync(int filterEspecialidadId = 0)
        {
            var filtrarEspecialidadesQuery = "";

            if (filterEspecialidadId > 0)
            {
                filtrarEspecialidadesQuery = $"&filtrarEspecialidadId={filterEspecialidadId}";
            }

            var response = await HttpRepo.Get<ApiResponseDto<MedicoResponse>>($"{_getUrl}{filtrarEspecialidadesQuery}");

            if (response is null)
            {
                return;
            }

            if (response.Error)
            {
                NotificationService.ShowError("Error!", "Error al cargar los datos");
            }
            else
            {
                _medicoList = response.Response.DataResult;
                _totalCount = response.Response.TotalCount;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

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
                _especialidades = httpResponse.Response;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadMedicoListAsync();
        }

        private async Task DeleteMedico(string id)
        {
            if (int.TryParse(id, out var idMedico))
            {
                if (idMedico > 0)
                {
                    var httpResp = await HttpRepo.Delete($"api/Medico/eliminar/{id}");

                    if (httpResp is null)
                    {
                        return;
                    }

                    if (httpResp.Error)
                    {
                        NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                    }
                    else
                    {
                        NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                        await LoadMedicoListAsync();
                    }
                }
            }
        }

        private async Task FilterByEspecialidad(object value)
        {
            try
            {
                await LoadMedicoListAsync(Convert.ToInt32(value));
            }
            catch (Exception)
            {
                NotificationService.ShowError("Error!", "Error al obtener el Id de la especialidad");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}