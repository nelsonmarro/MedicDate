﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Models
{
    public class Actividad : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required] [StringLength(500)] public string Nombre { get; set; }

        public List<ActividadCita> ActividadesCitas { get; set; }
    }
}