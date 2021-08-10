using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoEditar
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter]
        public string Id { get; set; }

        private MedicoRequest _medicoModel;
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<MedicoRequest>($"api/Medico/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                _medicoModel = httpResp.Response;
            }
        }

        private async Task EditMedico()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Medico/editar/{Id}", _medicoModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
                NavigationManager.NavigateTo("medicoList");
            }
        }
    }
}