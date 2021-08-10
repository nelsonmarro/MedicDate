using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadCrear : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        private bool _isBussy;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private readonly EspecialidadRequest _especialidadModel = new();

        private async Task CreateEspecialidad()
        {
            _isBussy = true;

            var httpResp =
                await HttpRepo.Post("api/Especialidad/crear",
                    _especialidadModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
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