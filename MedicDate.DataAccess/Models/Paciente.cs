using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Models
{
    public class Paciente : IId
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(150)] public string Nombres { get; set; }
        [Required] [StringLength(150)] public string Apellidos { get; set; }
        [Required] [StringLength(10)] public TipoSexo Sexo { get; set; }
        [Required] public int NumHistoria { get; set; }
        [Required] [StringLength(10)] public string Cedula { get; set; }
        [Required] public DateTime FechaNacimiento { get; set; }
        [StringLength(20)] public string Telefono { get; set; }
        [Required] [StringLength(100)] public string Email { get; set; }
        [StringLength(300)] public string Direccion { get; set; }

        public List<ArchivoPaciente> ArchivosPaciente { get; set; }
    }

    public enum TipoSexo
    {
        Masculino,
        Femenino
    }
}