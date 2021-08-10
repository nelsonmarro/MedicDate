using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadEditar : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        [Parameter] public string Id { get; set; }

        private bool _isBussy;

        private EspecialidadRequest _especialidadModel;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        protected override async Task OnParametersSetAsync()
        {
            var httpResp = await HttpRepo.Get<EspecialidadRequest>($"api/Especialidad/{Id}");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                _especialidadModel = httpResp.Response;
            }
        }

        private async Task EditEspecialidad()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Especialidad/editar/{Id}",
                _especialidadModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
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