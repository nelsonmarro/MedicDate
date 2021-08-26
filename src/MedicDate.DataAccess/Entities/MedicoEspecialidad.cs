namespace MedicDate.DataAccess.Entities
{
    public class MedicoEspecialidad
    {
        public string MedicoId { get; set; }
        public string EspecialidadId { get; set; }

        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}