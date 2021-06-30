using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadEditar : ComponentBase, IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        [Parameter] public string Id { get; set; }

        private EspecialidadRequest _especialidadModel = new();

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        protected override async Task OnParametersSetAsync()
        {
            var httpResp = await HttpRepo.Get<EspecialidadRequest>($"api/Especialidad/{Id}");

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
                _especialidadModel = httpResp.Response;
            }
        }

        private async Task Editar()
        {
            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp =
                await HttpRepo.Put($"api/Especialidad/editar/{Id}",
                    _especialidadModel);

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
                NotificationService.CloseDialog(DialogService);

                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("especialidadList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}