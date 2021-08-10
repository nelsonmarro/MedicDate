using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadEditar
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public string Id { get; set; }

        private bool _isBussy;

        private EspecialidadRequest _especialidadModel;

        protected override async Task OnParametersSetAsync()
        {
            var httpResp = await HttpRepo.Get<EspecialidadRequest>($"api/Especialidad/{Id}");

            if (!httpResp.Error)
            {
                _especialidadModel = httpResp.Response;
            }
        }

        private async Task EditEspecialidad()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Especialidad/editar/{Id}",
                _especialidadModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("especialidadList");
            }
        }
    }
}