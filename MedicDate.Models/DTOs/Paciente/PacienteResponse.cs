using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicDate.Models.DTOs.Grupo;
using MedicDate.Utility;
using MedicDate.Utility.Enums;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Models.DTOs.Paciente
{
    public class PacienteResponse : IId
    {
        public string Id { get; set; }
        public string NumHistoria { get; set; }
        public string Nombres { get; set; } 
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public string Cedula { get; set; }

        public int Edad => AgeCalculator.GetAge(FechaNacimiento);

        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public List<GrupoResponse> Grupos { get; set; }
    }
}