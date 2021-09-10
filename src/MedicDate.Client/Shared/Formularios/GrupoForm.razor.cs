using MedicDate.API.DTOs.Grupo;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios
{
    public partial class GrupoForm
    {
        [Parameter] public GrupoRequestDto GrupoModel { get; set; } = new();
        [Parameter] public EventCallback OnSubmit { get; set; }
    }
}
