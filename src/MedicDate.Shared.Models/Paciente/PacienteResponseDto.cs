using MedicDate.API.DTOs.Grupo;
using MedicDate.Utility.Extensions;
using MedicDate.Utility.Interfaces;
using System;
using System.Collections.Generic;

namespace MedicDate.API.DTOs.Paciente
{
    public class PacienteResponseDto : IId
    {
        public string Id { get; set; }
        public string NumHistoria { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public string Cedula { get; set; }

        public int Edad => FechaNacimiento.GetAge();

        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public List<GrupoResponseDto> Grupos { get; set; }
    }
}