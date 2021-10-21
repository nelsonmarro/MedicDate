using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Especialidad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadCrear
    {
        [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public INotificationService NotificationService { get; set; } = default!;

        private bool _isBussy;

        private readonly EspecialidadRequestDto _especialidadModel = new();

        private async Task CreateEspecialidad()
        {
            _isBussy = true;

            var httpResp =
                await HttpRepo.Post("api/Especialidad/crear",
                    _especialidadModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", "Especialidad creada con éxito");

                NavigationManager.NavigateTo("especialidadList");
            }
        }
    }
}