using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Pages.Cita.Components;
using MedicDate.Client.Services.IServices;
using MedicDate.Client.ViewModels;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Archivo;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Common.Extensions;
using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Cita.CalendarioCitas;

public partial class EditCita : ComponentBase
{
  private readonly List<ActividadCitaResponseDto> _selectedActividades = new();
  private List<ActividadResponseDto>? _actividades;
  private IEnumerable<string?> _actividadesIds = new List<string?>();
  private ActividadesTable? _actividadesTable;

  private CitaRequestDto? _citaRequest;
  private bool _isBussy;
  private List<ArchivoCitaVm>? _loadedArchivosPaciente;
  private List<MedicoCitaResponseDto>? _medicos;
  private List<PacienteCitaResponseDto>? _pacientes;

  [Inject]
  public IHttpRepository HttpRepo { get; set; } = default!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Parameter]
  public string? Id { get; set; }

  protected override async Task OnInitializedAsync()
  {
    _isBussy = true;

    _actividades = await RequestResourceListAsync<ActividadResponseDto>("api/Actividad/listar");
    _pacientes = await RequestResourceListAsync<PacienteCitaResponseDto>("api/Paciente/listar");
    _medicos = await RequestResourceListAsync<MedicoCitaResponseDto>("api/Medico/listar");

    if (!string.IsNullOrEmpty(Id))
    {
      var httpResp = await HttpRepo.Get<CitaRequestDto>($"api/Cita/obtenerParaEditar/{Id}");

      if (!httpResp.Error && httpResp.Response is not null)
      {
        _citaRequest = httpResp.Response;
        if (_citaRequest.ActividadesCita.Count > 0)
          _actividadesIds = _citaRequest.ActividadesCita.Select(x => x.ActividadId).ToList();
      }
    }

    CreateSelectedActividadesList();
    _isBussy = false;
  }

  private async Task LoadPacienteImages()
  {
    var httpResp = await HttpRepo.Get<List<ArchivoResponseDto>>($"api/Archivo/listAllByCita/{Id}");

    if (!httpResp.Error && httpResp.Response is not null)
      _loadedArchivosPaciente = httpResp.Response
        .Select(
          x =>
            new ArchivoCitaVm
            {
              Descripcion = x.Descripcion,
              Id = x.Id,
              RutaArchivo = x.RutaArchivo
            }
        )
        .ToList();

    StateHasChanged();
  }

  private async Task<List<T>?> RequestResourceListAsync<T>(string? reqUrl)
  {
    var httpResp = await HttpRepo.Get<List<T>>(reqUrl ?? "");
    return httpResp.Response;
  }

  private bool ValidateDatesForEdit()
  {
    if (_citaRequest is null)
      return false;

    var timeInicio = TimeOnly.FromDateTime(_citaRequest.FechaInicio.LocalDateTime);
    var timeFin = TimeOnly.FromDateTime(_citaRequest.FechaFin.LocalDateTime);
    if (timeInicio.Hour < 8 || timeFin.Hour > 20)
    {
      NotificationService.ShowError(
        "Error!",
        "Los tiempos de inicio y fin deben estar en el rango de 8:00 - 20:00"
      );
      return false;
    }

    if (_citaRequest.FechaInicio.LocalDateTime.IsTimeEquals(_citaRequest.FechaFin.LocalDateTime))
    {
      NotificationService.ShowError(
        "Error!",
        "Los tiempos de inicio y fin de la cita no pueden ser iguales"
      );
      return false;
    }

    if (_citaRequest.FechaInicio > _citaRequest.FechaFin)
    {
      NotificationService.ShowError(
        "Error!",
        "La fecha de inicio debe ser menor que la fecha de finalización"
      );

      return false;
    }

    return true;
  }

  private async Task UpdateCita()
  {
    if (_citaRequest is null)
    {
      NotificationService.ShowError("Error", "Error al actualizar la cita");
      return;
    }

    if (ValidateDatesForEdit())
    {
      _citaRequest.ActividadesCita = _selectedActividades
        .Select(
          x =>
            new ActividadCitaRequestDto
            {
              ActividadId = x.ActividadId,
              ActividadTerminada = x.ActividadTerminada,
              Detalles = x.Detalles
            }
        )
        .ToList();

      if (
        _citaRequest.Estado != Sd.ESTADO_CITA_NOASISTIOPACIENTE
        && _citaRequest.Estado != Sd.ESTADO_CITA_CANCELADA
        && _citaRequest.Estado != Sd.ESTADO_CITA_ANULADA
      )
      {
        _citaRequest.Estado = _citaRequest.ActividadesCita.TrueForAll(x => x.ActividadTerminada)
          ? Sd.ESTADO_CITA_COMPLETADA
          : Sd.ESTADO_CITA_CONFIRMADA;
      }

      var httpResp = await HttpRepo.Put($"api/Cita/editar/{Id}", _citaRequest);

      if (!httpResp.Error)
      {
        NotificationService.ShowSuccess("Operación exitosa", await httpResp.GetResponseBody());

        NavigationManager.NavigateTo(
          $"calendarioCitas?StartDate={new DateTime(day: 1, month: _citaRequest.FechaInicio.Month, year: _citaRequest.FechaInicio.Year)}&EndDate={new DateTime(day: 1, month: _citaRequest.FechaInicio.Month, year: _citaRequest.FechaInicio.Year).AddMonths(1)}"
        );
      }
    }
  }

  private async Task SelectActividad(object? value)
  {
    _citaRequest?.ActividadesCita.Clear();
    _actividadesIds = (IEnumerable<string>?)value ?? Array.Empty<string>();

    var actividadesIds = _actividadesIds.ToList();

    if (!actividadesIds.Any())
    {
      _selectedActividades.Clear();

      if (_actividadesTable is not null && _actividadesTable.DataGridRef is not null)
        await _actividadesTable.DataGridRef.Reload();

      return;
    }

    _citaRequest?.ActividadesCita.AddRange(
      actividadesIds.Select(actId => new ActividadCitaRequestDto { ActividadId = actId })
    );

    if (_selectedActividades.Count > actividadesIds.Count())
    {
      var actividadesToDelete = _selectedActividades
        .Where(x => !_actividadesIds.Contains(x.ActividadId))
        .ToList();

      await RemoveActividadFromSelectedListAsync(actividadesToDelete);
    }
    else
    {
      var actividadesIdsToAdd = actividadesIds.Except(
        _selectedActividades.Select(x => x.ActividadId)
      );

      var actividadesToAdd = _actividades
        ?.Select(
          x =>
            new ActividadCitaResponseDto
            {
              ActividadId = x.Id,
              ActividadTerminada = false,
              Detalles = "",
              Nombre = x.Nombre
            }
        )
        .Where(x => actividadesIdsToAdd.Contains(x.ActividadId));

      if (actividadesToAdd is not null)
        await AddActividadToSelectedListAsync(actividadesToAdd.ToList());
    }
  }

  private void CreateSelectedActividadesList()
  {
    if (_citaRequest is null)
      return;

    if (_actividades is not null)
      _selectedActividades.AddRange(
        _actividades.Join(
          _citaRequest.ActividadesCita,
          x => x.Id,
          y => y.ActividadId,
          (Z, q) =>
            new ActividadCitaResponseDto
            {
              ActividadId = q.ActividadId,
              ActividadTerminada = q.ActividadTerminada,
              Detalles = q.Detalles,
              Nombre = Z.Nombre
            }
        )
      );
  }

  private async Task AddActividadToSelectedListAsync(List<ActividadCitaResponseDto> actividades)
  {
    _selectedActividades.AddRange(actividades);

    if (_actividadesTable is not null && _actividadesTable.DataGridRef is not null)
      await _actividadesTable.DataGridRef.Reload();
  }

  private async Task RemoveActividadFromSelectedListAsync(
    List<ActividadCitaResponseDto> actividades
  )
  {
    foreach (var actividad in actividades)
      _selectedActividades.Remove(actividad);

    if (_actividadesTable?.DataGridRef != null)
      await _actividadesTable.DataGridRef.Reload();
  }

  private async Task RemoveActividadFromSelectedListAsync(ActividadCitaResponseDto actividad)
  {
    _selectedActividades.Remove(actividad);

    if (_actividadesTable?.DataGridRef != null)
      await _actividadesTable.DataGridRef.Reload();
  }

  private async Task RemoveActividadFromTableAsync(ActividadCitaResponseDto actividad)
  {
    await RemoveActividadFromSelectedListAsync(actividad);

    if (_actividadesIds is not null && _actividadesIds.Any())
      _actividadesIds = _actividadesIds.Where(id => id != actividad.ActividadId).ToList();
  }

  private async Task SavePacienteImagesAsync(List<ArchivoCitaVm> archivosCita)
  {
    var archivosCitaRequest = archivosCita.Select(
      x =>
        new CreateArchivoRequestDto
        {
          CitaId = Id,
          Descripcion = x.Descripcion,
          ExtensionImage = x.Extension,
          ImageBase64 = x.ImageInfo.ImgBase64,
          ContentType = x.ImageInfo.MimeType
        }
    );

    _loadedArchivosPaciente?.AddRange(archivosCita);
    var httpResp = await HttpRepo.Post("api/Archivo/subirListado", archivosCitaRequest);

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operación Exitosa", "Imagenes guardadas correctamente");

      await LoadPacienteImages();
    }
  }

  private void NavigateToCalendarioCitas()
  {
    NavigationManager.NavigateTo(
      $"calendarioCitas?StartDate={new DateTime(day: 1, month: _citaRequest!.FechaInicio.Month, year: _citaRequest.FechaInicio.Year)}&EndDate={new DateTime(day: 1, month: _citaRequest.FechaInicio.Month, year: _citaRequest.FechaInicio.Year).AddMonths(1)}"
    );
  }
}
