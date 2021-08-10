using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Models.DTOs.Grupo;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class GrupoForm
    {
        [Parameter] public GrupoRequest GrupoModel { get; set; } = new();
        [Parameter] public EventCallback OnSubmit { get; set; }

        private bool _popUp = true;
    }
}
