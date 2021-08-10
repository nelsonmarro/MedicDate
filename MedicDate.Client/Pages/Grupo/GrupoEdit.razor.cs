using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Grupo;


namespace MedicDate.Client.Pages.Grupo
{
    public partial class GrupoEdit : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        [Parameter] public string Id { get; set; }

        private bool _isBussy;

        private GrupoRequest _grupoModel;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        protected override async Task OnParametersSetAsync()
        {
            var httpResp = await HttpRepo.Get<GrupoRequest>($"api/Grupo/{Id}");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                _grupoModel = httpResp.Response;
            }
        }

        private async Task EditGrupo()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Grupo/editar/{Id}", _grupoModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("grupoList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
