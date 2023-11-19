﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Domain.Entities;

public class Paciente : BaseEntity, IId
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [StringLength(150)]
  public string Nombres { get; set; } = default!;

  [StringLength(150)]
  public string Apellidos { get; set; } = default!;

  [StringLength(10)]
  public string Sexo { get; set; } = default!;

  [StringLength(1000)]
  public string NumHistoria { get; set; } = default!;

  [StringLength(10)]
  public string Cedula { get; set; } = default!;

  public DateTime FechaNacimiento { get; set; }

  [StringLength(20)]
  public string? Telefono { get; set; }

  [StringLength(100)]
  public string Email { get; set; } = default!;

  [StringLength(300)]
  public string? Direccion { get; set; }

  public List<GrupoPaciente>? GruposPacientes { get; set; }
  public List<Cita> Citas { get; set; } = default!;
}
