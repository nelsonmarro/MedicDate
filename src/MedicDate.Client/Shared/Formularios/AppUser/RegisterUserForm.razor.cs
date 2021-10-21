using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.AppRole;
using MedicDate.Shared.Models.Auth;
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
