namespace MedicDate.Domain.Entities;

public class GrupoPaciente : BaseEntity
{
  public string PacienteId { get; set; } = default!;
  public string GrupoId { get; set; } = default!;

  public Paciente? Paciente { get; set; }
  public Grupo? Grupo { get; set; }
}