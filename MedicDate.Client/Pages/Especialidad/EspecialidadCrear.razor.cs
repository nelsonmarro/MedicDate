using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadCrear
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private bool _isBussy;

        private readonly EspecialidadRequest _especialidadModel = new();

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