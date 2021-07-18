using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Models.DTOs.Auth;


namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserCreate : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        private bool _isBussy;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private readonly RegisterRequest _registerModel = new();

        private async Task CreateUser()
        {
            if (_registerModel.RolesIds.Count == 0)
            {
                await DialogNotificationService.ShowError("Error!", "Debe seleccionar al menos un rol para el usuario");

                return;
            }

            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Account/register", _registerModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("usuarioList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
