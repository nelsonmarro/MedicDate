using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.AppRole;
using MedicDate.Shared.Models.AppUser;
using MedicDate.Shared.Models.Clinica;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios.AppUser;

public partial class EditUserForm
{
    [Inject] public IHttpRepository HttpRepo { get; set; } = default!;

    [Inject]
    public INotificationService NotificationService { get; set; } = default!;

    [Parameter] public AppUserRequestDto EditUserModel { get; set; } = new();
    [Parameter] public EventCallback OnSubmit { get; set; }

    private readonly string[] _headers = { "Nombre Rol", "Descripción" };
    private readonly string[] _propName = { "Nombre", "Descripcion" };
    private bool _emailNeedConfirmation;

    private List<RoleResponseDto>? _roleList;
    private List<RoleResponseDto> _selectedRoles = new();
    private List<ClinicaResponseDto>? _clinicas;

    protected override async Task OnInitializedAsync()
    {
        var httpResp =
          await HttpRepo.Get<List<RoleResponseDto>>("api/Usuario/roles");

        if (httpResp.Error)
            NotificationService.ShowError("Error!",
              "Error al obtener los roles");
        else
            _roleList = httpResp.Response;

        _selectedRoles?.AddRange(_roleList!.Where(x => EditUserModel.Roles.Any(y => y.Id == x.Id)));
        _emailNeedConfirmation = EditUserModel.EmailConfirmed;
    }

    protected override void OnParametersSet()
    {

    }

    private async Task OnSubmitData()
    {
        if (_selectedRoles is not null)
            if (_selectedRoles.Count > 0)
                EditUserModel.Roles = _selectedRoles;
        await OnSubmit.InvokeAsync();
    }

    private async Task SendConfirmationEmail()
    {
        var httpResp = await HttpRepo.Post(
          "api/Account/sendConfirmationEmail", EditUserModel.Email);

        if (httpResp is null) return;

        if (httpResp.Error)
            NotificationService.ShowError("Error!",
              await httpResp.GetResponseBody());
        else
            NotificationService.ShowSuccess("Operación exitosa!",
              "Email de confirmación enviado correctamente");
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
            EditUserModel.ClinicaId = _clinicas?.FirstOrDefault()?.Id;
        }
        else
        {
            EditUserModel.ClinicaId = string.Empty;
        }
    }
}