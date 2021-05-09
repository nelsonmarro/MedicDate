using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class EspecialidadForm
    {
        [Parameter] public EspecialidadRequest EspecialidadModel { get; set; } = new();
        [Parameter] public EventCallback OnSubmit { get; set; }
    }
}
