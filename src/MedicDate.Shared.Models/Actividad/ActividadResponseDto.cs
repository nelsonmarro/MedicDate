﻿using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Shared.Models.Actividad;

public class ActividadResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string? Nombre { get; set; }
}