using System;

namespace MedicDate.DataAccess.Models
{
    public class GrupoPaciente
    {
        public int PacienteId { get; set; }
        public int GrupoId { get; set; }

        public Paciente Paciente { get; set; }
        public Grupo Grupo { get; set; }
    }
}
