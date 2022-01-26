using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Clinica;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Clinica;

public partial class ClinicaCrear : ComponentBase
{
    private readonly ClinicaRequestDto _clinicaModel = new();

    private bool _isBussy;
    [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public INotificationService NotificationService { get; set; } = default!;

    private async Task CreateClinica()
    {
        _isBussy = true;

        var httpResp =
          await HttpRepo.Post("api/Clinica/crear", _clinicaModel);

        _isBussy = false;

        if (!httpResp.Error)
        {
            NotificationService.ShowSuccess("Operación Exitosa!",
              "Clínica creada con éxito");

            NavigationManager.NavigateTo("clinicaList");
        }
    }
}
