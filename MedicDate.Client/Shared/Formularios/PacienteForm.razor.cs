using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Grupo;
using MedicDate.Models.DTOs.Paciente;
using MedicDate.Utility.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class PacienteForm : ComponentBase
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter]
        public PacienteRequest PacienteRequest { get; set; } = new();

        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<GrupoResponse> _grupos;
        private IEnumerable<string> _gruposIds;

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await HttpRepo.Get<List<GrupoResponse>>("api/Grupo/listar");

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
            if (PacienteRequest.GruposId.Count > 0)
            {
                _gruposIds = PacienteRequest.GruposId;
            }
        }

        private void SelectGrupo(object value)
        {
            PacienteRequest.GruposId.Clear();

            var gruposIds = (IEnumerable<string>)value;

            if (gruposIds is not null)
            {
                PacienteRequest.GruposId.AddRange(gruposIds);
            }
        }
    }
}
