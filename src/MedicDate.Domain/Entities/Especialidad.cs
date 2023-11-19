﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Domain.Entities;

public class Especialidad : BaseEntity, IId
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [StringLength(100)] public string NombreEspecialidad { get; set; } = default!;

  public List<MedicoEspecialidad> MedicosEspecialidades { get; set; } =
    default!;
}