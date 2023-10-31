using MedicDate.Client.Components;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Especialidad;
using MedicDate.Shared.Models.Medico;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios;

public partial class MedicoForm
{
  [Inject]
  public IHttpRepository HttpRepo { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Parameter]
  public MedicoRequestDto MedicoRequestDto { get; set; } = new();

  [Parameter]
  public EventCallback OnSubmit { get; set; }

  [Parameter]
  public bool ShowCancelLink { get; set; } = true;

  [Parameter]
  public EventCallback OnCancel { get; set; }

  private RadzenDropDownWithPopup<List<string>, EspecialidadResponseDto> _especialidadDropdown =
    default!;

  private List<EspecialidadResponseDto>? _especialidades;
  private List<string>? _especialidadesIds;
  private bool _isBussy;

  private EspecialidadRequestDto _especialidadRequestDto { get; set; } = new();

  private async Task<List<EspecialidadResponseDto>?> GetEspecialidades()
  {
    var httpResponse = await HttpRepo.Get<List<EspecialidadResponseDto>>("api/Especialidad/listar");

    if (httpResponse.Error)
      NotificationService.ShowError("Error!", await httpResponse.GetResponseBody());

    return httpResponse.Response;
  }

  protected override async Task OnInitializedAsync()
  {
    _especialidades = await GetEspecialidades();
  }

  protected override void OnParametersSet()
  {
    if (MedicoRequestDto.EspecialidadesId.Count > 0)
      _especialidadesIds = MedicoRequestDto.EspecialidadesId;
  }

  private void SelectEspecialidad(object value)
  {
    MedicoRequestDto.EspecialidadesId.Clear();

    var especialidades = (List<string>)value;

    if (especialidades is not null)
      MedicoRequestDto.EspecialidadesId.AddRange(especialidades);
  }

  private async Task AddEspecialidad()
  {
    _isBussy = true;

    var httpResp = await HttpRepo.Post("api/Especialidad/crear", _especialidadRequestDto);

    _isBussy = false;

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operación Exitosa!", "Especialidad creada con éxito");
      _especialidades = await GetEspecialidades();
      await RadzenPopupHelper.ClosePopup(_especialidadDropdown);
      _especialidadRequestDto = new();
    }
  }
}
