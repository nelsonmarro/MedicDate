using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility.Interfaces;
using System.Collections.Generic;

namespace MedicDate.Models.DTOs.Medico
{
    public class MedicoResponse : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string PhoneNumber { get; set; }
        public List<EspecialidadResponse> Especialidades { get; set; }
    }
}