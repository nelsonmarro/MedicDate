using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Authorization;
using Radzen.Blazor;

namespace MedicDate.Client.Pages.Especialidad
{
    [Authorize]
    public partial class EspecialidadList : ComponentBase, IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        private List<EspecialidadResponse> _especialidadList;
        private readonly string[] _cabecerasTable = { "Nombre" };
        private readonly string[] _nombreProps = { "NombreEspecialidad" };
        private const string GetUrl = "api/Especialidad/listarConPaginacion";
        private int _totalCount;

        private PermitirOpCrud _permitirOp = new()
        { PermitirAgregar = true, PermitirEditar = true, PermitirEliminar = true };

        private RutasOp _rutasOp = new()
        { AgregarUrl = "especialidadCrear", EditarUrl = "especialidadEditar", GetUrl = GetUrl };

        private async Task CargarEspecialidades()
        {
            var response = await HttpRepo.Get<ApiResponseDto<EspecialidadResponse>>(GetUrl);

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
                _especialidadList = response.Response.DataResult;
                _totalCount = response.Response.TotalCount;
            }
        }

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        protected override async Task OnParametersSetAsync()
        {
            await CargarEspecialidades();
        }

        private async Task EliminarEspecialidad(string id)
        {
            if (int.TryParse(id, out var idEspe))
            {
                if (idEspe > 0)
                {
                    var httpResp = await HttpRepo.Delete($"api/Especialidad/eliminar/{idEspe}");

                    if (httpResp.Error)
                    {
                        NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                    }
                    else
                    {
                        NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                        await CargarEspecialidades();
                    }
                }
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}