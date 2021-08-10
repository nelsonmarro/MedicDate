using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Paciente;


namespace MedicDate.Client.Pages.Paciente
{
    public partial class PacienteCreate : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IDialogNotificationService DialogNotificationService { get; set; }

        private readonly PacienteRequest _pacienteModel = new();
        private bool _isBussy;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private async Task CreatePaciente()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Paciente/crear", _pacienteModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                await DialogNotificationService
                    .ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operacion exitosa!", "Registro creado con éxito");

                NavigationManager.NavigateTo("pacienteList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
