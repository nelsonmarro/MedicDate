using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Medico;


namespace MedicDate.Client.Shared.Formularios
{
    public partial class MedicoForm
    {
        [Parameter] public MedicoRequest MedicoRequest { get; set; } = new();
        [Parameter] public EventCallback OnSubmit { get; set; }
    }
}
