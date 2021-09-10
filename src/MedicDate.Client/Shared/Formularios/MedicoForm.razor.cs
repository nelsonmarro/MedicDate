using MedicDate.API.DTOs.Especialidad;
using MedicDate.API.DTOs.Medico;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class MedicoForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public MedicoRequestDto MedicoRequestDto { get; set; } = new();

        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<EspecialidadResponseDto> _especialidades;
        private IEnumerable<string> _especialidadesIds;

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await HttpRepo.Get<List<EspecialidadResponseDto>>("api/Especialidad/listar");

            if (httpResponse.Error)
            {
                NotificationService.ShowError("Error!", await httpResponse.GetResponseBody());
            }
            else
            {
                _especialidades = httpResponse.Response;
            }
        }

        protected override void OnParametersSet()
        {
            if (MedicoRequestDto.EspecialidadesId.Count > 0) _especialidadesIds = MedicoRequestDto.EspecialidadesId;
        }

        private void SelectEspecialidad(object value)
        {
            MedicoRequestDto.EspecialidadesId.Clear();

            var especialidades = (IEnumerable<string>)value;

            if (especialidades is not null) MedicoRequestDto.EspecialidadesId.AddRange(especialidades);
        }
    }
}