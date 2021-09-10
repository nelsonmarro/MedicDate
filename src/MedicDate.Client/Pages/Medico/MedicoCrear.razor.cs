using MedicDate.API.DTOs.Medico;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoCrear
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private readonly MedicoRequestDto _medicoModel = new();
        private bool _isBussy;

        private async Task CreateMedico()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Medico/crear", _medicoModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operacion exitosa!", "Registro creado con éxito");

                NavigationManager.NavigateTo("medicoList"); ;
            }
        }
    }
}