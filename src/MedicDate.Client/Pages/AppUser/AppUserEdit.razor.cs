using MedicDate.API.DTOs.AppUser;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserEdit
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        [Parameter] public string Id { get; set; }

        private AppUserRequestDto _appUserModel;
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<AppUserRequestDto>($"api/Usuario/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                _appUserModel = httpResp.Response;
            }
        }

        private async Task EditUser()
        {
            if (_appUserModel.Roles.Count == 0)
            {
                await DialogNotificationService
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
                NavigationManager.NavigateTo("usuarioList");
            }
        }
    }
}
