using System;
using System.ComponentModel.DataAnnotations;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Models
{
    public class Paciente : IId
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(150)] public string Nombres { get; set; }
        [Required] [StringLength(150)] public string Apellidos { get; set; }
        [Required] [StringLength(10)] public string Cedula { get; set; }
        [Required] public int Edad { get; set; }
        [Required] public DateTime FechaNacimiento { get; set; }
        [StringLength(20)] public string Telefono { get; set; }
        [StringLength(300)] public string Direccion { get; set; }
        [StringLength(150)] public string Ciudad { get; set; }
    }
}