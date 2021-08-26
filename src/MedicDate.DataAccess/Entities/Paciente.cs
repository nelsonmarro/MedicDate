using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities
{
    public class Paciente : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] public string Id { get; set; }
        [Required] [StringLength(150)] public string Nombres { get; set; }
        [Required] [StringLength(150)] public string Apellidos { get; set; }
        [Required] [StringLength(10)] public string Sexo { get; set; }
        [Required] [StringLength(1000)] public string NumHistoria { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Cedula { get; set; }

        [Required] public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Telefono { get; set; }

        [Required] [StringLength(100)] public string Email { get; set; }
        [StringLength(300)] public string Direccion { get; set; }

        public List<GrupoPaciente> GruposPacientes { get; set; }
        public List<Cita> Citas { get; set; }
    }
}