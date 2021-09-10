using MedicDate.API.DTOs.Grupo;
using MedicDate.API.DTOs.Paciente;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class PacienteForm : ComponentBase
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter] public PacienteRequestDto PacienteRequestDto { get; set; } = new();

        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<GrupoResponseDto> _grupos;
        private IEnumerable<string> _gruposIds;

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await HttpRepo.Get<List<GrupoResponseDto>>("api/Grupo/listar");

            if (httpResponse.Error)
            {
                NotificationService.ShowError("Error!", await httpResponse.GetResponseBody());
            }
            else
            {
                _grupos = httpResponse.Response;
            }
        }

        protected override void OnParametersSet()
        {
            if (PacienteRequestDto.GruposId.Count > 0) _gruposIds = PacienteRequestDto.GruposId;
        }

        private void SelectGrupo(object value)
        {
            PacienteRequestDto.GruposId.Clear();

            var gruposIds = (IEnumerable<string>)value;

            if (gruposIds is not null) PacienteRequestDto.GruposId.AddRange(gruposIds);
        }
    }
}
