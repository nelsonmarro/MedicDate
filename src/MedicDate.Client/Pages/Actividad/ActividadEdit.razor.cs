using MedicDate.API.DTOs.Actividad;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Actividad
{
    public partial class ActividadEdit
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public string Id { get; set; }

        private bool _isBussy;

        private ActividadRequestDto _actividadModel;

        protected override async Task OnParametersSetAsync()
        {
            var httpResp = await HttpRepo
                .Get<ActividadRequestDto>($"api/Actividad/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                _actividadModel = httpResp.Response;
            }
        }

        private async Task EditActividad()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Actividad/editar/{Id}", _actividadModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("actividadList");
            }
        }
    }
}
