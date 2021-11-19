using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Medico;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Medico;

public partial class MedicoEditar : IDisposable
{
  private bool _isBussy;

  private MedicoRequestDto? _medicoModel;
  [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
  [Inject] public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Inject]
  public IHttpInterceptorProvider HttpInterceptorProvider { get; set; } =
    default!;

  [Parameter] public string? Id { get; set; }

  protected override async Task OnInitializedAsync()
  {
    HttpInterceptorProvider.AuthTokenInterceptor.RegisterEvent();
    HttpInterceptorProvider.ErrorInterceptor.RegisterEvent();

    var httpResp =
      await HttpRepo.Get<MedicoRequestDto>(
        $"api/Medico/obtenerParaEditar/{Id}");

    if (!httpResp.Error) _medicoModel = httpResp.Response;
  }

  private async Task EditMedico()
  {
    _isBussy = true;

    var httpResp = await HttpRepo.Put($"api/Medico/editar/{Id}", _medicoModel);

    _isBussy = false;

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operación exitosa!",
        "Registro editado con éxito");
      NavigationManager.NavigateTo("medicoList");
    }
  }

  public void Dispose()
  {
    HttpInterceptorProvider.AuthTokenInterceptor.DisposeEvent();
    HttpInterceptorProvider.ErrorInterceptor.DisposeEvent();
  }
}