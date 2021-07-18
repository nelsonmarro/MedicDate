using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Grupo
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(150)] public string Nombre { get; set; }
    }
}
