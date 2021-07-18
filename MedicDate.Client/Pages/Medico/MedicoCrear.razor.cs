using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoCrear : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        private readonly MedicoRequest _medicoModel = new();
        private bool _isBussy;

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private async Task CreateMedico()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post<MedicoRequest, MedicoResponse>("api/Medico/crear", _medicoModel);

            _isBussy = false;

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operacion exitosa!", "Registro creado con éxito");

                NavigationManager.NavigateTo("medicoList");
            }
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}