using System.ComponentModel.DataAnnotations;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Models.DTOs.Especialidad
{
    public class EspecialidadResponse : IId
    {
        public string Id { get; set; }
        public string NombreEspecialidad { get; set; }
    }
}