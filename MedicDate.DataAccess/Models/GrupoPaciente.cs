using System;

namespace MedicDate.DataAccess.Models
{
    public class GrupoPaciente
    {
        public string PacienteId { get; set; }
        public string GrupoId { get; set; }

        public Paciente Paciente { get; set; }
        public Grupo Grupo { get; set; }
    }
}
