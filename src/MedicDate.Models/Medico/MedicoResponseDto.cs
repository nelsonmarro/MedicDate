using System.Collections.Generic;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.Utility.Interfaces;

namespace MedicDate.API.DTOs.Medico
{
    public class MedicoResponseDto : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string PhoneNumber { get; set; }
        public List<EspecialidadResponseDto> Especialidades { get; set; }
    }
}