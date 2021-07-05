using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Client.Shared;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility;
using Radzen;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadCrear : ComponentBase, IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private EspecialidadRequest _especialidadModel = new();

        private async Task CreateEspecialidad()
        {
            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp =
                await HttpRepo.Post("api/Especialidad/crear",
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

                NotificationService.ShowSuccess("Operación Exitosa!", "Especialidad creada con éxito");

                NavigationManager.NavigateTo("especialidadList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}