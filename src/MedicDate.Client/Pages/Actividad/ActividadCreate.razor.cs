using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Actividad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Actividad
{
    public partial class ActividadCreate
    {
        [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public INotificationService NotificationService { get; set; } = default!;

        private bool _isBussy;

        private readonly ActividadRequestDto _actividadModel = new();

        private async Task CreateActividad()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Actividad/crear", _actividadModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", "Actividad creada con éxito");

                NavigationManager.NavigateTo("actividadList");
            }
        }
    }
}
