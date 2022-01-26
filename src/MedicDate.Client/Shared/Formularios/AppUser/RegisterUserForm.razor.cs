using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.AppRole;
using MedicDate.Shared.Models.Auth;
using MedicDate.Shared.Models.Clinica;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios.AppUser
{
    public partial class RegisterUserForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject] public INotificationService NotificationService { get; set; } = default!;

        [Parameter] public RegisterUserDto RegisterModel { get; set; } = new();
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public string[]? Errors { get; set; }

        private List<RoleResponseDto>? _roleList;
        private readonly List<RoleResponseDto> _selectedRoles = new();
        private List<ClinicaResponseDto>? _clinicas;

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
            if (_selectedRoles.Count > 0)
            {
                RegisterModel.RolesIds = _selectedRoles.Select(x => x.Id).ToList();
            }

            await OnSubmit.InvokeAsync();
        }

        private async Task LoadClinicas()
        {
            var httpResp = await HttpRepo.Get<List<ClinicaResponseDto>>("api/Clinica/listar");

            if (!httpResp.Error)
            {
                _clinicas = httpResp.Response;
            }
        }

        private async Task OnValueSelected(RoleResponseDto? selectedRole)
        {
            if (selectedRole is not null && selectedRole.Nombre != Sd.ROLE_ADMIN_GENERAL)
            {
                if (_clinicas is null)
                {
                    await LoadClinicas();
                }
                RegisterModel.ClinicaId = _clinicas?.FirstOrDefault()?.Id;
            }
            else
            {
                RegisterModel.ClinicaId = string.Empty;
            }
        }
    }
}
