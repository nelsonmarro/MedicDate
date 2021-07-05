using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios.AppUser
{
    public partial class RegisterUserForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public RegisterRequest RegisterModel { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<RoleResponse> _roleList;
        private readonly List<RoleResponse> _selectedRoles = new();

        private readonly string[] _headers = {"Nombre Rol", "Descripción"};
        private readonly string[] _propName = {"Nombre", "Descripcion"};

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<List<RoleResponse>>("api/Usuario/roles");

            if (httpResp is null)
            {
                return;
            }

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", "Error al obtener los roles");
            }
            else
            {
                _roleList = httpResp.Response;
            }
        }

        private async Task OnSubmitData()
        {
            Console.WriteLine(_selectedRoles.Count);

            if (_selectedRoles.Count > 0)
            {
                RegisterModel.RolesIds = _selectedRoles.Select(x => x.Id).ToList();
            }

            await OnSubmit.InvokeAsync();
        }
    }
}
