namespace MedicDate.DataAccess.Entities
{
    public class MedicoEspecialidad
    {
        public string MedicoId { get; set; } = default!;
        public string EspecialidadId { get; set; } = default!;

        public Medico Medico { get; set; } = default!;
        public Especialidad Especialidad { get; set; } = default!;
    }
}