using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Pages.Cita.Components;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace MedicDate.Client.Pages.Cita.CalendarioCitas
{
    public partial class CalendarioCitas
    {
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

        [Parameter]
        [SupplyParameterFromQuery]
        public string StartDate { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string EndDate { get; set; } = default!;

        private IEnumerable<CitaCalendarDto>? _citasCalendar = new List<CitaCalendarDto>();
        private RadzenScheduler<CitaCalendarDto> _scheduler = default!;

        private async Task LoadCitas(SchedulerLoadDataEventArgs e)
        {
            DateTime start;
            DateTime end;

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {

                DateTime parsedStartDate = DateTime.Parse(StartDate);
                DateTime parsedEndDate = DateTime.Parse(EndDate);

                start = parsedStartDate;
                end = parsedEndDate;
            }
            else
            {
                start = e.Start;
                end = e.End;
            }

            var httpResp = await HttpRepo.Get<IEnumerable<CitaCalendarDto>>
                ($"api/Cita/listarPorFechas?startDate={start}&endDate={end}");

            if (!httpResp.Error)
            {
                _citasCalendar = httpResp.Response;
                _scheduler.CurrentDate = start;

                if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                {
                    StartDate = "";
                    EndDate = "";
                }
            }
        }

        private async Task OnSlotSelect(SchedulerSlotSelectEventArgs e)
        {
            var citoToSave = await DialogService.OpenAsync<AddCitaDialog>("Agregar Cita", new Dictionary<string, object>
            {
                {"StartDate", e.Start},
                {"EndDate", e.End}
            });

            if (citoToSave is not null)
            {
                await SaveSelectedCita(citoToSave);
            }
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

        private async Task OnMenuItemClick(MenuItemEventArgs args, CitaCalendarDto cita)
        {
            if (args.Value is null)
            {
                return;
            }

            switch (args.Value.ToString())
            {
                case "Anulada":
                    cita.Estado = Sd.ESTADO_CITA_ANULADA;
                    await UpdateCitaEstado(cita.Id, cita.Estado);
                    break;

                case "Confirmada":
                    cita.Estado = Sd.ESTADO_CITA_CONFIRMADA;
                    await UpdateCitaEstado(cita.Id, cita.Estado);
                    break;

                case "Cancelada":
                    cita.Estado = Sd.ESTADO_CITA_CANCELADA;
                    await UpdateCitaEstado(cita.Id, cita.Estado);
                    break;

                case "No asistió paciente":
                    cita.Estado = Sd.ESTADO_CITA_NOASISTIOPACIENTE;
                    await UpdateCitaEstado(cita.Id, cita.Estado);
                    break;

                case "Por Confirmar":
                    cita.Estado = Sd.ESTADO_CITA_PORCONFIRMAR;
                    await UpdateCitaEstado(cita.Id, cita.Estado);
                    break;

                default:
                    break;
            }
        }

        private void ShowContextMenuWithContent(MouseEventArgs args, CitaCalendarDto cita) => ContextMenuService.Open(args, ds => DisplayCitaEstadosMenu(cita));

        private void OnCitaRender(SchedulerAppointmentRenderEventArgs<CitaCalendarDto> e)
        {
            switch (e.Data.Estado)
            {
                case Sd.ESTADO_CITA_ANULADA:
                    e.Attributes["style"] = $"background: #ee6c4d";
                    break;

                case Sd.ESTADO_CITA_CONFIRMADA:
                    e.Attributes["style"] = $"background: #52b69a";
                    break;

                case Sd.ESTADO_CITA_CANCELADA:
                    e.Attributes["style"] = $"background: #d62828";
                    break;

                case Sd.ESTADO_CITA_NOASISTIOPACIENTE:
                    e.Attributes["style"] = $"background: #f77f00";
                    break;

                case Sd.ESTADO_CITA_PORCONFIRMAR:
                    e.Attributes["style"] = $"background: #3ba5fc";
                    break;

                default:
                    break;
            }
        }

        private async Task UpdateCitaEstado(string? citaId, string? newEstado)
        {
            await HttpRepo.Put($"api/Cita/actualizarEstado/{citaId}",
            newEstado);
            await _scheduler.Reload();
        }
    }
}