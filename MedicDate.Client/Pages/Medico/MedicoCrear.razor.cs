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
    public partial class MedicoCrear : ComponentBase, IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        private MedicoRequest _medicoModel = new();

        protected override void OnInitialized()
        {
            HttpInterceptor.RegisterEvent();
        }

        private async Task CreateMedico()
        {
            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp = await HttpRepo.Post<MedicoRequest, MedicoResponse>("api/Medico/crear", _medicoModel);

            NotificationService.CloseDialog(DialogService);

            if (httpResp is null)
            {
                return;
            }

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