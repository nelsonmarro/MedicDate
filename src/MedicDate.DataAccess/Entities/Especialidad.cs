using MedicDate.Utility.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Especialidad : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; } = default!;

        [StringLength(100)]
        public string NombreEspecialidad { get; set; } = default!;

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; } = default!;
    }
}