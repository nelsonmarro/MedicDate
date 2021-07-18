using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.AppUser;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MedicDate.Client.Shared.Formularios.AppUser
{
    public partial class EditUserForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public AppUserRequest EditUserModel { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<RoleResponse> _roleList;
        private List<RoleResponse> _selectedRoles;
        private bool _emailNeedConfirmation;

        private readonly string[] _headers = {"Nombre Rol", "Descripción"};
        private readonly string[] _propName = {"Nombre", "Descripcion"};

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<List<RoleResponse>>("api/Usuario/roles");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", "Error al obtener los roles");
            }
            else
            {
                _roleList = httpResp.Response;
            }
        }

        protected override void OnParametersSet()
        {
            _selectedRoles = EditUserModel.Roles;
            _emailNeedConfirmation = !EditUserModel.EmailConfirmed;
        }

        private async Task OnSubmitData()
        {
            if (_selectedRoles.Count > 0)
            {
                EditUserModel.Roles = _selectedRoles;
            }

            await OnSubmit.InvokeAsync();
        }

        private async Task SendConfirmationEmail()
        {
            var httpResp = await HttpRepo.Post("api/Account/sendConfirmationEmail", EditUserModel.Email);

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
                NotificationService.ShowSuccess("Operación exitosa!", "Email de confirmación enviado correctamente");
            }
        }
    }
}