using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.AppUser;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserEdit
    {
        [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public INotificationService NotificationService { get; set; } = default!;
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; } = default!;

        [Parameter] public string? Id { get; set; }

        private AppUserRequestDto _appUserModel = new();
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<AppUserRequestDto>($"api/Usuario/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                if (httpResp.Response is not null)
                {
                    _appUserModel = httpResp.Response;
                }
            }
        }

        private async Task EditUser()
        {
            if (_appUserModel.Roles.Count == 0)
            {
                await DialogNotificationService!
                    .ShowError("Error!", "Debe seleccionar al menos un rol para el usuario");

                return;
            }

            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Usuario/editar/{Id}", _appUserModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!",
                    "Registro editado con éxito");
                NavigationManager!.NavigateTo("usuarioList");
            }
        }
    }
}
