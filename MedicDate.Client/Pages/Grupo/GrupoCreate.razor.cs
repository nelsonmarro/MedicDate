using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Grupo;

namespace MedicDate.Client.Pages.Grupo
{
    public partial class GrupoCreate : IDisposable
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

        private readonly GrupoRequest _grupoModel = new();

        private async Task CreateGrupo()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Grupo/crear", _grupoModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operación Exitosa!", "Grupo creado con éxito");

                NavigationManager.NavigateTo("grupoList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
