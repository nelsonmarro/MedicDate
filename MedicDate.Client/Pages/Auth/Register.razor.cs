using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Auth;
using Radzen;


namespace MedicDate.Client.Pages.Auth
{
    public partial class Register
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IAuthenticationService AuthService { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        private RegisterRequest _registerRequest = new();
        private bool _showRegisterationErrors;
        public IEnumerable<string> Errors { get; set; }

        public async Task RegisterUser()
        {
            NotificationService.ShowLoadingDialog(DialogService);
            _showRegisterationErrors = false;

            var result = await AuthService.RegisterUser(_registerRequest);

            NotificationService.CloseDialog(DialogService);

            if (!result.IsRegisterSuccessful)
            {
                Errors = result.Errors;
                _showRegisterationErrors = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
