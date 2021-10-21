using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Paciente
{
    public partial class PacienteEdit
    {
        [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public INotificationService NotificationService { get; set; } = default!;
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; } = default!;

        [Parameter] public string? Id { get; set; }

        private PacienteRequestDto? _pacienteModel;
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<PacienteRequestDto>($"api/Paciente/obtenerParaEditar/{Id}");

            if (!httpResp.Error)
            {
                _pacienteModel = httpResp.Response;
            }
        }

        private async Task EditPaciente()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Paciente/editar/{Id}", _pacienteModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
                NavigationManager.NavigateTo("pacienteList");
            }
        }
    }
}