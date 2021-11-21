using System.Globalization;
using MedicDate.Client.Components;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Pages.Cita.Components;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace MedicDate.Client.Pages.Cita.CalendarioCitas;

public partial class CalendarioCitas
{
   private List<CitaCalendarDto>? _citasCalendar;
   private RadzenScheduler<CitaCalendarDto> _scheduler = default!;

   [Inject] public IHttpRepository HttpRepo { get; set; } = default!;

   [Inject]
   public INotificationService NotificationService { get; set; } = default!;

   [Inject] public DialogService DialogService { get; set; } = default!;

   [Inject] public TooltipService TooltipService { get; set; } = default!;

   [Inject]
   public ContextMenuService ContextMenuService { get; set; } = default!;

   [Inject] public NavigationManager NavigationManager { get; set; } = default!;

   [Parameter]
   [SupplyParameterFromQuery]
   public string? StartDate { get; set; }

   [Parameter]
   [SupplyParameterFromQuery]
   public string? EndDate { get; set; }

   private string? _medicoId;
   private string? _pacienteId;
   private string _startDate = "";
   private string _endDate = "";
   private bool _refreshForFilter = false;

   protected override async Task OnParametersSetAsync()
   {
      _startDate = StartDate ?? "";
      _endDate = EndDate ?? "";

      if (_refreshForFilter)
      {
         await LoadCitas();
      }

      _refreshForFilter = false;
   }

   private async Task LoadCitas(SchedulerLoadDataEventArgs? e = null)
   {
      var start = DateTime.Now;
      var end = DateTime.Now;

      if (!string.IsNullOrEmpty(_startDate) && !string.IsNullOrEmpty(_endDate))
      {
         var parsedStartDate = DateTime.Parse(_startDate);
         var parsedEndDate = DateTime.Parse(_endDate);

         start = parsedStartDate;
         end = parsedEndDate;
      }
      else if (e is not null)
      {
         start = e.Start;
         end = e.End;
      }

      var httpResp = await HttpRepo.Get<List<CitaCalendarDto>>
         ($"api/Cita/listarPorFechas?startDate={start}&endDate={end}&medicoId={_medicoId}&pacienteId={_pacienteId}");

      if (!httpResp.Error)
      {
         _citasCalendar = httpResp.Response;

         if (!string.IsNullOrEmpty(_startDate) && !string.IsNullOrEmpty(_endDate))
         {
            _startDate = "";
            _endDate = "";
         }
      }
   }

   private async Task OnSlotSelect(SchedulerSlotSelectEventArgs e)
   {
      var citoToSave = await DialogService.OpenAsync<AddCitaDialog>(
        "Agregar Cita", new Dictionary<string, object>
        {
        {"StartDate", e.Start},
        {"EndDate", e.End}
        });

      if (citoToSave is not null) await SaveSelectedCita(citoToSave);
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
            new Dictionary<string, object>
            {
             {
                "DetailsText",
                "¿Está seguro/a de eliminar esta cita?"
             }
            },
            new DialogOptions { Width = "465px", Height = "280px" });

      if (permitirEliminar)
      {
         await SendDeleteCitaReqAsync(citaId);
      }
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
          new DialogOptions { Width = "485px", Height = "300px" });
   }

   private async Task OnMenuItemClick(MenuItemEventArgs args,
     CitaCalendarDto cita)
   {
      if (args.Value is null) return;

      if (args.Text == "Eliminar Cita")
      {
         await DeleteCitaAsync(args.Value.ToString() ?? "");
         return;
      }

      if (cita.Estado == Sd.ESTADO_CITA_COMPLETADA)
      {
         if (!await OpenChangeCitaCompletadaEstadoDialogAsync())
            return;
      }

      switch (args.Value.ToString())
      {
         case "Anulada":
            cita.Estado = Sd.ESTADO_CITA_ANULADA;
            await TryUpdateCitaEstadoAsync(cita.Id, cita.Estado);
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
      NotificationService
         .ShowSuccess("Operación Existosa", "Cita eliminada correctamente");
   }

   private void ShowContextMenuWithContent(MouseEventArgs args,
    CitaCalendarDto cita)
   {
      ContextMenuService.Open(args, ds => DisplayCitaEstadosMenu(cita));
   }

   private void OnCitaRender(
     SchedulerAppointmentRenderEventArgs<CitaCalendarDto> e)
   {
      if (e.Data is not null)
      {
         switch (e.Data.Estado)
         {
            case Sd.ESTADO_CITA_ANULADA:
               e.Attributes["style"] = "background: #ee6c4d";
               break;

            case Sd.ESTADO_CITA_CONFIRMADA:
               e.Attributes["style"] = "background: #8236CB";
               break;

            case Sd.ESTADO_CITA_CANCELADA:
               e.Attributes["style"] = "background: #d62828";
               break;

            case Sd.ESTADO_CITA_NOASISTIOPACIENTE:
               e.Attributes["style"] = "background: #f77f00";
               break;

            case Sd.ESTADO_CITA_PORCONFIRMAR:
               e.Attributes["style"] = "background: #3ba5fc";
               break;

            case Sd.ESTADO_CITA_COMPLETADA:
               e.Attributes["style"] = "background: #3E7C17";
               break;
         }
      }
   }

   private async Task UpdateCitaEstadoAndRefreshScheduler(string? citaId, string? newEstado)
   {
      var result = await HttpRepo.Put($"api/Cita/actualizarEstado/{citaId}",
        newEstado);

      if (!result.Error)
      {
         await _scheduler.Reload();
      }
   }

   private async Task<bool> TryUpdateCitaEstadoAsync(string? citaId, string? newEstado)
   {
      var result = await HttpRepo.Put($"api/Cita/actualizarEstado/{citaId}",
        newEstado);

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
        {"startDate", startDate.ToString("G", CultureInfo.CurrentCulture)},
        {"endDate", endDate.ToString("G", CultureInfo.CurrentCulture)}
     };

      var newUri = NavigationManager.GetUriWithQueryParameters(queryStrings);
      NavigationManager.NavigateTo(newUri);
      _refreshForFilter = true;
   }
}