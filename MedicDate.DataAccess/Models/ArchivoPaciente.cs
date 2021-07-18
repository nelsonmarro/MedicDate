using System;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class ArchivoPaciente
    {
        [Key] public int Id { get; set; }
        [Required] public string RutaArchivo { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
    }
}
