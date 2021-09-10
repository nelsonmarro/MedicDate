using MedicDate.API.DTOs.Auth;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserCreate
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        private bool _isBussy;
        private readonly RegisterUserDto _registerModel = new();
        private string[] _errors = Array.Empty<string>();

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
                _errors = await httpResp.HttpResponseMessage.Content.ReadFromJsonAsync<string[]>();
                NotificationService.ShowError("Error!", "Error al crear el usuario");
            }
            else
            {
                NavigationManager.NavigateTo("usuarioList");
            }
        }
    }
}