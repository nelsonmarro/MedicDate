using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Auth;
using Radzen;

namespace MedicDate.Client.Pages.Auth
{
    public partial class Login
    {
        private LoginRequest _loginRequest = new();

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IAuthenticationService AuthenticationService { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        private bool _showAuthError;
        public string Error { get; set; }

        public async Task ExecuteLogin()
        {
            NotificationService.ShowLoadingDialog(DialogService);
            _showAuthError = false;

            var result = await AuthenticationService.Login(_loginRequest);

            NotificationService.CloseDialog(DialogService);

            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                _showAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
