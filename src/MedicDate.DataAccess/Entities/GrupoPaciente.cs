namespace MedicDate.DataAccess.Entities;

public class GrupoPaciente
{
    public string PacienteId { get; set; } = default!;
    public string GrupoId { get; set; } = default!;

    public Paciente Paciente { get; set; }
    public Grupo Grupo { get; set; } = default!;
}