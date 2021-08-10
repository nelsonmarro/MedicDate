using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Paciente;


namespace MedicDate.Client.Pages.Paciente
{
    public partial class PacienteEdit : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        [Parameter] public string Id { get; set; }

        private PacienteRequest _pacienteModel;
        private bool _isBussy;

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

            var httpResp = await HttpRepo.Get<PacienteRequest>($"api/Paciente/obtenerParaEditar/{Id}");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                _pacienteModel = httpResp.Response;
            }
        }

        private async Task EditPaciente()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Put($"api/Paciente/editar/{Id}", _pacienteModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                await DialogNotificationService.ShowError("Error!", await httpResp.GetResponseBody());

                return;
            }

            NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
            NavigationManager.NavigateTo("pacienteList");
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}