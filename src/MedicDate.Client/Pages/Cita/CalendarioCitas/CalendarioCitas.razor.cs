using System.Globalization;
using MedicDate.Client.Components;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Pages.Cita.Components;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace MedicDate.Client.Pages.Cita.CalendarioCitas;

public partial class CalendarioCitas : IDisposable
{
  private List<CitaCalendarDto>? _citasCalendar;
  private string _endDate = "";

  private string? _medicoId;
  private string? _pacienteId;
  private bool _refreshForFilter;
  private RadzenScheduler<CitaCalendarDto> _scheduler = default!;
  private string _startDate = "";

  [Inject]
  public IHttpRepository HttpRepo { get; set; } = default!;

  [Inject]
  public INotificationService NotificationService { get; set; } = default!;

  [Inject]
  public DialogService DialogService { get; set; } = default!;

  [Inject]
  public TooltipService TooltipService { get; set; } = default!;

  [Inject]
  public ContextMenuService ContextMenuService { get; set; } = default!;

  [Inject]
  public NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  public IJSRuntime jsRuntime { get; set; } = default!;

  [Parameter]
  [SupplyParameterFromQuery]
  public string? StartDate { get; set; }

  [Parameter]
  [SupplyParameterFromQuery]
  public string? EndDate { get; set; }

  public void Dispose()
  {
    var jsInProcess = (IJSInProcessRuntime)jsRuntime;
    jsInProcess.InvokeVoid("changeBodyContainerHeightToMaxVh");
    ContextMenuService.Close();
  }

  protected override void OnAfterRender(bool firstRender)
  {
    if (!firstRender)
      return;
    var jsInProcess = (IJSInProcessRuntime)jsRuntime;
    jsInProcess.InvokeVoid("changeBodyContainerHeight");
  }

  protected override async Task OnParametersSetAsync()
  {
    _startDate = StartDate ?? "";
    _endDate = EndDate ?? "";

    if (_refreshForFilter)
      await LoadCitas();

    _refreshForFilter = false;
  }

  private async Task LoadCitas(SchedulerLoadDataEventArgs? e = null)
  {
    if (!string.IsNullOrEmpty(_startDate) && !string.IsNullOrEmpty(_endDate))
    {
      var httpResp = await HttpRepo.Get<List<CitaCalendarDto>>(
        $"api/Cita/listarPorFechas?startDate={_startDate}&endDate={_endDate}&medicoId={_medicoId}&pacienteId={_pacienteId}"
      );

      if (!httpResp.Error)
      {
        _citasCalendar = httpResp.Response;

        if (!string.IsNullOrEmpty(_startDate) && !string.IsNullOrEmpty(_endDate))
        {
          _scheduler.CurrentDate = DateTime.Parse(_startDate, CultureInfo.CurrentCulture);
          _startDate = "";
          _endDate = "";
        }
      }
    }
    else if (e is not null)
    {
      var httpResp = await HttpRepo.Get<List<CitaCalendarDto>>(
        $"api/Cita/listarPorFechas?startDate={e.Start}&endDate={e.End}&medicoId={_medicoId}&pacienteId={_pacienteId}"
      );

      if (!httpResp.Error)
        _citasCalendar = httpResp.Response;
    }
  }

  private async Task OnSlotSelect(SchedulerSlotSelectEventArgs e)
  {
    if (e.View.Text == "Año")
      return;

    var dialogOptions = new DialogOptions { Width = "780px", Height = "420px" };

    var citaToSave = await DialogService.OpenAsync<AddCitaDialog>(
      "Agregar Cita",
      new Dictionary<string, object> { { "StartDate", e.Start }, { "EndDate", e.End } },
      dialogOptions
    );

    if (citaToSave is not null)
      await SaveSelectedCita(citaToSave);
  }

  private void OnSlotRender(SchedulerSlotRenderEventArgs args)
  {
    // Highlight today in month view
    if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
      args.Attributes["style"] = "background: rgba(255,220,40,.2);";

    // Highlight working hours (9-18)
    if (
      (args.View.Text == "Week" || args.View.Text == "Day")
      && args.Start.Hour > 8
      && args.Start.Hour < 21
    )
      args.Attributes["style"] = "background: rgba(255,220,40,.2);";
  }

  private async Task SaveSelectedCita(CitaRequestDto citaRequestDto)
  {
    citaRequestDto.Estado = Sd.ESTADO_CITA_PORCONFIRMAR;

    var httpResp = await HttpRepo.Post("api/Cita/crear", citaRequestDto);

    if (!httpResp.Error)
    {
      NotificationService.ShowSuccess("Operación Exitosa", "Cita Registrada");
      await _scheduler.Reload();
    }
  }

  private async Task DeleteCitaAsync(string citaId)
  {
    var permitirEliminar = await DialogService.OpenAsync<DeleteConfirmation>(
      "Eliminar Cita",
      new Dictionary<string, object> { { "DetailsText", "¿Está seguro/a de eliminar esta cita?" } },
      new DialogOptions { Width = "465px", Height = "280px" }
    );

    if (permitirEliminar)
      await SendDeleteCitaReqAsync(citaId);
  }

  private async Task<bool> OpenChangeCitaCompletadaEstadoDialogAsync()
  {
    return await DialogService.OpenAsync<DeleteConfirmation>(
      "Cambiar Estado",
      new Dictionary<string, object>
      {
        {
          "DetailsText",
          "Esta cita ya está marcada como completada!. ¿Está seguro/a de cambiar su estado?"
        }
      },
      new DialogOptions { Width = "485px", Height = "300px" }
    );
  }

  private async Task OnMenuItemClick(MenuItemEventArgs args, CitaCalendarDto cita)
  {
    if (args.Value is null)
      return;

    if (args.Value.ToString() == cita.Estado)
      return;

    if (args.Value.ToString() == "close")
    {
      ContextMenuService.Close();
      return;
    }

    if (args.Text == "Eliminar Cita")
    {
      ContextMenuService.Close();
      await DeleteCitaAsync(args.Value.ToString() ?? "");
      return;
    }

    if (args.Value.ToString() == "editar")
    {
      ContextMenuService.Close();
      NavigationManager.NavigateTo($"citaEditar/{cita.Id}");
      return;
    }

    if (cita.Estado == Sd.ESTADO_CITA_COMPLETADA)
      if (!await OpenChangeCitaCompletadaEstadoDialogAsync())
        return;

    switch (args.Value.ToString())
    {
      case "Anulada":
        cita.Estado = Sd.ESTADO_CITA_ANULADA;
        var result = await TryUpdateCitaEstadoAsync(cita.Id, cita.Estado);
        if (result)
        {
          cita.Estado = Sd.ESTADO_CITA_ANULADA;
          await _scheduler.Reload();
        }

        break;

      case "Confirmada":
        cita.Estado = Sd.ESTADO_CITA_CONFIRMADA;
        await UpdateCitaEstadoAndRefreshScheduler(cita.Id, cita.Estado);
        break;

      case "Cancelada":
        cita.Estado = Sd.ESTADO_CITA_CANCELADA;
        await UpdateCitaEstadoAndRefreshScheduler(cita.Id, cita.Estado);
        break;

      case "No asistió paciente":
        cita.Estado = Sd.ESTADO_CITA_NOASISTIOPACIENTE;
        await UpdateCitaEstadoAndRefreshScheduler(cita.Id, cita.Estado);
        break;

      case "Por Confirmar":
        cita.Estado = Sd.ESTADO_CITA_PORCONFIRMAR;
        await UpdateCitaEstadoAndRefreshScheduler(cita.Id, cita.Estado);
        break;

      case "Completada":
        if (await TryUpdateCitaEstadoAsync(cita.Id, Sd.ESTADO_CITA_COMPLETADA))
        {
          cita.Estado = Sd.ESTADO_CITA_COMPLETADA;
          await _scheduler.Reload();
        }

        break;
    }
  }

  private async Task SendDeleteCitaReqAsync(string citaId)
  {
    await HttpRepo.Delete($"api/Cita/eliminar/{citaId}");
    await _scheduler.Reload();
    NotificationService.ShowSuccess("Operación Existosa", "Cita eliminada correctamente");
  }

  private void ShowContextMenuWithContent(MouseEventArgs args, CitaCalendarDto cita)
  {
    ContextMenuService.Open(args, ds => DisplayCitaEstadosMenu(cita));
  }

  private static void OnCitaRender(SchedulerAppointmentRenderEventArgs<CitaCalendarDto> e)
  {
    if (e.Data is null)
      return;
    e.Attributes["style"] = e.Data.Estado switch
    {
      Sd.ESTADO_CITA_ANULADA => "background: #5584AC",
      Sd.ESTADO_CITA_CONFIRMADA => "background: #8236CB",
      Sd.ESTADO_CITA_CANCELADA => "background: #d62828",
      Sd.ESTADO_CITA_NOASISTIOPACIENTE => "background: #f77f00",
      Sd.ESTADO_CITA_PORCONFIRMAR => "background: #3ba5fc",
      Sd.ESTADO_CITA_COMPLETADA => "background: #3E7C17",
      _ => e.Attributes["style"]
    };
  }

  private async Task UpdateCitaEstadoAndRefreshScheduler(string? citaId, string? newEstado)
  {
    var result = await HttpRepo.Put($"api/Cita/actualizarEstado/{citaId}", newEstado);

    if (!result.Error)
      await _scheduler.Reload();
  }

  private async Task<bool> TryUpdateCitaEstadoAsync(string? citaId, string? newEstado)
  {
    var result = await HttpRepo.Put($"api/Cita/actualizarEstado/{citaId}", newEstado);

    return !result.Error;
  }

  private async Task FilterCitasByMedicoOrPaciente((string? PacienteId, string? MedicoId) filterIds)
  {
    var (pacienteId, medicoId) = filterIds;

    _pacienteId = pacienteId;
    _medicoId = medicoId;

    await _scheduler.Reload();
  }

  private void FiltarCitasByDates((DateTime StartDate, DateTime EndDate) filterDates)
  {
    var (startDate, endDate) = filterDates;

    var queryStrings = new Dictionary<string, object?>
    {
      { "startDate", startDate.ToString("G", CultureInfo.CurrentCulture) },
      { "endDate", endDate.ToString("G", CultureInfo.CurrentCulture) }
    };

    var newUri = NavigationManager.GetUriWithQueryParameters(queryStrings);
    NavigationManager.NavigateTo(newUri);
    _refreshForFilter = true;
  }
}
