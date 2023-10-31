using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Paciente;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MedicDate.Client.Pages
{
  public partial class Index : ComponentBase, IDisposable
  {
    [Inject]
    public IHttpRepository HttpRepo { get; set; } = null!;
    [Inject]
    public INotificationService NotifService { get; set; } = null!;
    [Inject]
    public IJSRuntime jsRuntime { get; set; } = null!;

    private List<CitaEstadoMonthReviewDto>? _citasCompletadas;
    private List<CitaEstadoMonthReviewDto>? _citasCanceladas;
    private List<CitaEstadoMonthReviewDto>? _citasNoAsistdas;

    private List<CitaRegisteredQuarterReviewDto>? _citasRegisteredQuarter;

    private List<PacienteMonthReviewDto>? _pacientesPreviousYear;
    private List<PacienteMonthReviewDto>? _pacientesPresentYear;
    private readonly string _initialCitasReq = $"calendarioCitas?{new DateTime(day: 1, month: DateTime.Today.Month, year: DateTime.Today.Year)}&EndDate={new DateTime(day: 1, month: DateTime.Today.Month, year: DateTime.Today.Year).AddMonths(1)}";

    protected override async Task OnInitializedAsync()
    {
      _citasCompletadas = await
         GetCitaEstadoChartItemListAsync(Sd.ESTADO_CITA_COMPLETADA, DateTime.Now.Year);
      _citasCanceladas = await
         GetCitaEstadoChartItemListAsync(Sd.ESTADO_CITA_CANCELADA, DateTime.Now.Year);
      _citasNoAsistdas = await
         GetCitaEstadoChartItemListAsync(Sd.ESTADO_CITA_NOASISTIOPACIENTE, DateTime.Now.Year);

      _citasRegisteredQuarter = await GetCitasAnualQuarterReview(DateTime.Now.Year);

      _pacientesPreviousYear = await
         GetPacientesAnualMonthReviewAsync(DateTime.Now.AddYears(-1).Year);
      _pacientesPresentYear = await
         GetPacientesAnualMonthReviewAsync(DateTime.Now.Year);
    }

    protected override void OnAfterRender(bool firstRender)
    {
      if (firstRender)
      {
        var jsInProcess = (IJSInProcessRuntime) jsRuntime;
        jsInProcess.InvokeVoid("changeBodyContainerHeight");
      }
    }

    private async Task<List<T>?> GetResourceListAsync<T>(string url)
    {
      var httpResp = await HttpRepo.Get<List<T>>
        ($"api/{url}");

      if (!httpResp.Error)
      {
        return httpResp.Response;
      }

      return new List<T>();
    }

    private async Task<List<CitaEstadoMonthReviewDto>?>
       GetCitaEstadoChartItemListAsync(string estado, int year)
    {
      return await GetResourceListAsync<CitaEstadoMonthReviewDto>
         ($"Cita/getEstadoReview/{year}/{estado}");
    }

    private async Task<List<PacienteMonthReviewDto>?>
       GetPacientesAnualMonthReviewAsync(int year)
    {
      return await GetResourceListAsync<PacienteMonthReviewDto>
         ($"Paciente/getAnualMonthReview/{year}");
    }

    private async Task<List<CitaRegisteredQuarterReviewDto>?>
       GetCitasAnualQuarterReview(int year)
    {
      return await GetResourceListAsync<CitaRegisteredQuarterReviewDto>
         ($"Cita/getQuarterReview/{year}");
    }

    public void Dispose()
    {
      var jsInProcess = (IJSInProcessRuntime) jsRuntime;
      jsInProcess.InvokeVoid("changeBodyContainerHeightToMaxVh");
    }
  }
}