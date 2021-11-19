using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Grupo;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Grupo;

public partial class GrupoEdit
{
  private GrupoRequestDto? _grupoModel;

  private bool _isBussy;
  [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
  [Inject] public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Parameter] public string? Id { get; set; }

  protected override async Task OnParametersSetAsync()
  {
    var httpResp = await HttpRepo
      .Get<GrupoRequestDto>($"api/Grupo/obtenerParaEditar/{Id}");

    if (!httpResp.Error) _grupoModel = httpResp.Response;
  }

  private async Task EditGrupo()
  {
    _isBussy = true;

    var httpResp = await HttpRepo.Put($"api/Grupo/editar/{Id}", _grupoModel);

    _isBussy = false;

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operación Exitosa!",
        await httpResp.GetResponseBody());

      NavigationManager.NavigateTo("grupoList");
    }
  }
}