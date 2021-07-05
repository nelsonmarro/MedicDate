using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Models.DTOs.AppUser;


namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserEdit : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        [Parameter] public string Id { get; set; }

        private AppUserRequest _appUserModel = new();

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

            var httpResp = await HttpRepo.Get<AppUserRequest>($"api/Usuario/obtenerParaEditar/{Id}");

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
                _appUserModel = httpResp.Response;
            }
        }

        private async Task EditUser()
        {
            if (_appUserModel.Roles.Count == 0)
            {
                await DialogNotificationService.ShowError("Error!", "Debe seleccionar al menos un rol para el usuario");

                return;
            }

            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp = await HttpRepo.Put($"api/Usuario/editar/{Id}", _appUserModel);

            NotificationService.CloseDialog(DialogService);

            if (httpResp is null)
            {
                return;
            }

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                return;
            }

            NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
            NavigationManager.NavigateTo("usuarioList");
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
