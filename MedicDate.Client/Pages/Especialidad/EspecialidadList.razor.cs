using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using Radzen.Blazor;


namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadList
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private List<EspecialidadResponse> _especialidadList;
        private string[] _cabecerasTable = {"Nombre"};
        private string[] _nombreProps = {"NombreEspecialidad"};
        private string _getUrl = "api/Especialidad/listarConPaginacion";
        private int _totalCount;

        protected override async Task OnInitializedAsync()
        {
            await CargarEspecialidades();
        }

        private async Task CargarEspecialidades()
        {
            var response = await HttpRepo.Get<ApiResponseDto<EspecialidadResponse>>(_getUrl);

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

        private async Task EliminarEspecialidad(int id)
        {
            if (id != 0)
            {
                var httpResp = await HttpRepo.Delete($"api/Especialidad/eliminar/{id}");

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
}