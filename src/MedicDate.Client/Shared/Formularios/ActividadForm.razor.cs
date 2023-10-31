﻿using MedicDate.Shared.Models.Actividad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios;

public partial class ActividadForm
{
  [Parameter]
  public ActividadRequestDto ActividadModel { get; set; } = new();

  [Parameter]
  public EventCallback OnSubmit { get; set; }

  [Parameter]
  public bool ShowCancelLink { get; set; } = true;

  [Parameter]
  public EventCallback OnCancel { get; set; }
}
