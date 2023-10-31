﻿using MedicDate.Shared.Models.Especialidad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Shared.Formularios;

public partial class EspecialidadForm
{
  [Parameter]
  public EspecialidadRequestDto EspecialidadModel { get; set; } = new();

  [Parameter]
  public EventCallback OnSubmit { get; set; }

  [Parameter]
  public bool ShowCancelLink { get; set; } = true;

  [Parameter]
  public EventCallback OnCancel { get; set; }
}
