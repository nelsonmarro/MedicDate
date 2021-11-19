using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Medico;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Medico;

public partial class MedicoCrear
{
  private readonly MedicoRequestDto _medicoModel = new();
  private bool _isBussy;
  [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
  [Inject] public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  private async Task CreateMedico()
  {
    _isBussy = true;

    var httpResp = await HttpRepo.Post("api/Medico/crear", _medicoModel);

    _isBussy = false;

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operacion exitosa!",
        "Registro creado con éxito");

      NavigationManager.NavigateTo("medicoList");
      ;
    }
  }
}