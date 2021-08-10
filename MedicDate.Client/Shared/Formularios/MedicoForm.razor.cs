using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class MedicoForm
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        [Parameter]
        public MedicoRequest MedicoRequest { get; set; } = new();

        [Parameter] public EventCallback OnSubmit { get; set; }

        private List<EspecialidadResponse> _especialidades;
        private IEnumerable<string> _especialidadesIds;

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await HttpRepo.Get<List<EspecialidadResponse>>("api/Especialidad/listar");

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
            if (MedicoRequest.EspecialidadesId.Count > 0)
            {
                _especialidadesIds = MedicoRequest.EspecialidadesId;
            }
        }

        private void SelectEspecialidad(object value)
        {
            MedicoRequest.EspecialidadesId.Clear();

            var especialidades = (IEnumerable<string>)value;

            if (especialidades is not null)
            {
                MedicoRequest.EspecialidadesId.AddRange(especialidades);
            }
        }
    }
}