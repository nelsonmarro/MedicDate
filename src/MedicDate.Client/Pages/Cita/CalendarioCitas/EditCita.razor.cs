using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;
using System.Linq.Dynamic.Core;

namespace MedicDate.Client.Pages.Cita.CalendarioCitas
{
    public partial class EditCita : ComponentBase
    {
        [Inject]
        public IHttpRepository HttpRepo { get; set; } = default!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public string? Id { get; set; }

        private CitaRequestDto _citaRequest = new();
        private IEnumerable<MedicoCitaResponseDto>? _medicos;
        private IEnumerable<PacienteCitaResponseDto>? _pacientes;
        private IEnumerable<ActividadResponseDto>? _actividades;
        private List<ActividadCitaResponseDto> _selectedActividades = new();
        private IEnumerable<string>? _actividadesIds;
        private bool _isBussy;

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
                    {
                        _actividadesIds = _citaRequest.ActividadesCita.Select(x => x.ActividadId).ToList();
                    }
                }
            }

            CreateSelectedActividadesList();
            _isBussy = false;
        }

        private async Task<IEnumerable<T>?> RequestResourceListAsync<T>(string? reqUrl)
        {
            var httpResp = await HttpRepo.Get<IEnumerable<T>>(reqUrl ?? "");
            return !httpResp.Error ? httpResp.Response : Enumerable.Empty<T>();
        }

        private void UpdateCita()
        {
        }

        private void SelectActividad(object value)
        {
            _citaRequest.ActividadesCita.Clear();
            _selectedActividades.Clear();

            _actividadesIds = (IEnumerable<string>)value;

            if (value is null)
            {
                _selectedActividades = _selectedActividades.ToList();
                return;
            }

            _citaRequest.ActividadesCita.AddRange(_actividadesIds
                .Select(actId => new ActividadCitaRequestDto { ActividadId = actId }));

            CreateSelectedActividadesList();
            _selectedActividades = _selectedActividades.ToList();
        }

        private void CreateSelectedActividadesList()
        {
            if (_actividades is not null && _actividadesIds is not null)
            {
                _selectedActividades.AddRange
                    (_actividades.Where(a =>
                    _actividadesIds.Any(x => x == a.Id))
                    .Select(ar => new ActividadCitaResponseDto
                    {
                        ActividadId = ar.Id,
                        ActividadTerminada = false,
                        Detalles = "",
                        Nombre = ar.Nombre
                    }));
            }
        }

        private void RemoveActividadFromSelectedList(ActividadCitaResponseDto actividad)
        {
            _selectedActividades = _selectedActividades.Where(x => x.ActividadId != actividad.ActividadId).ToList();

            if (_actividadesIds is not null && _actividadesIds.Any())
            {
                _actividadesIds = _actividadesIds.Where(id => id != actividad.ActividadId);
            }
        }

        private void NavigateToCalendarioCitas()
        {
            NavigationManager.NavigateTo($"calendarioCitas?StartDate={_citaRequest.FechaInicio}&EndDate={_citaRequest.FechaFin}");
        }
    }
}