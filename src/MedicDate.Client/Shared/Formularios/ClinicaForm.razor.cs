using MedicDate.Shared.Models.Clinica;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios;

public partial class ClinicaForm : ComponentBase
{
    [Parameter] public ClinicaRequestDto ClinicaModel { get; set; } = new();
    [Parameter] public EventCallback OnSubmit { get; set; }
}
