using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Medico
{
    public partial class MedicoEditar : IDisposable
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        [Parameter]
        public int Id { get; set; }

        private MedicoRequest _medicoModel;

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

            var httpResp = await HttpRepo.Get<MedicoRequest>($"api/Medico/obtenerParaEditar/{Id}");

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
                _medicoModel = httpResp.Response;
            }
        }

        private async Task EditMedico()
        {
            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp = await HttpRepo.Put($"api/Medico/editar/{Id}", _medicoModel);

            NotificationService.CloseDialog(DialogService);

            if (httpResp is null)
            {
                return;
            }

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                return;
            }

            NotificationService.ShowSuccess("Operación exitosa!", "Registro editado con éxito");
            NavigationManager.NavigateTo("medicoList");
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}