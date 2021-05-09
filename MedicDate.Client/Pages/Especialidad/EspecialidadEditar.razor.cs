using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Especialidad;


namespace MedicDate.Client.Pages.Especialidad
{
    public partial class EspecialidadEditar
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        [Parameter] public int Id { get; set; }

        private EspecialidadRequest _especialidadModel = new();

        protected override async Task OnInitializedAsync()
        {
            var httpResp = await HttpRepo.Get<EspecialidadRequest>($"api/Especialidad/{Id}");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                _especialidadModel = httpResp.Response;
            }
        }

        private async Task Editar()
        {
            NotificationService.ShowLoadingDialog(DialogService);

            var httpResp =
                await HttpRepo.Put($"api/Especialidad/editar/{Id}",
                    _especialidadModel);

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.CloseDialog(DialogService);

                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                NavigationManager.NavigateTo("especialidadList");
            }
        }
    }
}