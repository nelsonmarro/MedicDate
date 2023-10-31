using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Paciente;

public partial class PacienteCreate
{
  private readonly PacienteRequestDto _pacienteModel = new();
  private bool _isBussy;

  [Inject]
  public IHttpRepository HttpRepo { get; set; } = default!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Inject]
  public IDialogNotificationService DialogNotificationService { get; set; } = default!;

  private async Task CreatePaciente()
  {
    _pacienteModel.DateRegistered = DateTime.Now;

    _isBussy = true;

    var httpResp = await HttpRepo.Post("api/Paciente/crear", _pacienteModel);

    _isBussy = false;

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operacion exitosa!", "Registro creado con éxito");

      NavigationManager.NavigateTo("pacienteList");
    }
  }
}
