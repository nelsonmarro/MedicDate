using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Especialidad;
using MedicDate.Shared.Models.Medico;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios;

public partial class MedicoForm
{
  private List<EspecialidadResponseDto>? _especialidades;
  private IEnumerable<string>? _especialidadesIds;
  [Inject] public IHttpRepository HttpRepo { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Parameter] public MedicoRequestDto MedicoRequestDto { get; set; } = new();

  [Parameter] public EventCallback OnSubmit { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var httpResponse =
      await HttpRepo.Get<List<EspecialidadResponseDto>>(
        "api/Especialidad/listar");

    if (httpResponse.Error)
      NotificationService.ShowError("Error!",
        await httpResponse.GetResponseBody());
    else
      _especialidades = httpResponse.Response;
  }

  protected override void OnParametersSet()
  {
    if (MedicoRequestDto.EspecialidadesId.Count > 0)
      _especialidadesIds = MedicoRequestDto.EspecialidadesId;
  }

  private void SelectEspecialidad(object value)
  {
    MedicoRequestDto.EspecialidadesId.Clear();

    var especialidades = (IEnumerable<string>) value;

    if (especialidades is not null)
      MedicoRequestDto.EspecialidadesId.AddRange(especialidades);
  }
}