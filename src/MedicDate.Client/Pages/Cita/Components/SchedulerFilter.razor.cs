using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Cita.Components;

public partial class SchedulerFilter
{
    [Inject] public IHttpRepository HttpRepo { get; set; } = null!;
    [Inject] public INotificationService NotificationService { get; set; } = null!;

    [Parameter]
    public EventCallback<(string? PacienteId, string? MedicoId)> OnFilterSelected { get; set; }

    [Parameter]
    public EventCallback<(DateTime StartDate, DateTime EndDate)> OnDatesFilterSelected { get; set; }

    private List<MedicoCitaResponseDto> _medicos = new();

    private List<PacienteCitaResponseDto> _pacientes = new();

    private string? _medicoId;
    private string? _pacienteId;
    private DateTime _startDate = DateTime.Now;
    private DateTime _endDate = DateTime.Now;

    protected override async Task OnInitializedAsync()
    {
        _medicos =
           await SendGetResourceListRequestAsync<MedicoCitaResponseDto>("Medico/listar");

        _pacientes =
           await SendGetResourceListRequestAsync<PacienteCitaResponseDto>("Paciente/listar");
    }

    private async Task<List<TResponse>> SendGetResourceListRequestAsync<TResponse>(string url)
    {
        var httpResp =
           await HttpRepo.Get<List<TResponse>>($"api/{url}");

        return !httpResp.Error && httpResp.Response is not null
           ? httpResp.Response
           : new List<TResponse>();
    }

    private async Task OnMedicoSelectedAsync(object value)
    {
        _medicoId = (string) value;
        await OnFilterSelected.InvokeAsync((_pacienteId, _medicoId));
    }

    private async Task OnPacienteSelectedAsync(object value)
    {
        _pacienteId = (string) value;
        await OnFilterSelected.InvokeAsync((_pacienteId, _medicoId));
    }

    private bool ValidateDatesForRequest()
    {
        if (_startDate > _endDate)
        {
            NotificationService
               .ShowError("Error!", "La fecha de inicio debe ser menor que la fecha de finalizaci�n");

            return false;
        }

        return true;
    }

    private async Task FilterByDatesAsync()
    {
        if (ValidateDatesForRequest())
            await OnDatesFilterSelected.InvokeAsync((_startDate, _endDate));
    }

    private async Task GoToCurrentMonthAsync()
    {
        _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 10, 0, 0);
        _endDate = _startDate.AddMonths(1);
        await OnDatesFilterSelected.InvokeAsync((_startDate, _endDate));
    }
}