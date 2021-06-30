using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicDate.DataAccess.Models
{
    public class MedicoEspecialidad
    {
        public int MedicoId { get; set; }
        public int EspecialidadId { get; set; }

        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}