using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.API.DTOs.AppRole;
using MedicDate.API.DTOs.AppUser;
using MedicDate.API.DTOs.Auth;

namespace MedicDate.Client.Shared.Formularios.AppUser
{
    public partial class RegisterUserForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public RegisterUserDto RegisterModel { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public string[] Errors { get; set; }

        private List<RoleResponseDto> _roleList;
        private readonly List<RoleResponseDto> _selectedRoles = new();

        private readonly string[] _headers = { "Nombre Rol", "Descripción" };
        private readonly string[] _propName = { "Nombre", "Descripcion" };

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<List<RoleResponseDto>>("api/Usuario/roles");

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
