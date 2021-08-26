using System;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Medico;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoEditar : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorProvider HttpInterceptorProvider { get; set; }

        [Parameter]
        public string Id { get; set; }

        private MedicoRequestDto _medicoModel;
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptorProvider.AuthTokenInterceptor.RegisterEvent();
            HttpInterceptorProvider.ErrorInterceptor.RegisterEvent();

            var httpResp = await HttpRepo.Get<MedicoRequestDto>($"api/Medico/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                _medicoModel = httpResp.Response;
            }
        }

        private async Task EditMedico()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Medico/editar/{Id}", _medicoModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
                NavigationManager.NavigateTo("medicoList");
            }
        }

        public void Dispose()
        {
            HttpInterceptorProvider.AuthTokenInterceptor.DisposeEvent();
            HttpInterceptorProvider.ErrorInterceptor.DisposeEvent();
        }
    }
}