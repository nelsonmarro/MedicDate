using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Clinica;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Clinica;

public partial class ClinicaEditar : ComponentBase
{
    [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public INotificationService NotificationService { get; set; } = default!;

    [Parameter] public string? Id { get; set; }

    private bool _isBussy;

    private ClinicaRequestDto? _clinicaModel;

    protected override async Task OnParametersSetAsync()
    {
        var httpResp = await HttpRepo.Get<ClinicaRequestDto>($"api/Clinica/obtenerParaEditar/{Id}");

        if (!httpResp.Error)
        {
            _clinicaModel = httpResp.Response;
        }
    }

    private async Task EditClinica()
    {
        _isBussy = true;

        var httpResp = await HttpRepo.Put($"api/Clinica/editar/{Id}",
            _clinicaModel);

        _isBussy = false;

        if (!httpResp.Error)
        {
            NotificationService.ShowSuccess("Operación Exitosa!",
            await httpResp.GetResponseBody());

            NavigationManager.NavigateTo("clinicaList");
        }
    }
}
