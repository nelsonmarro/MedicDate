using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Archivo
{
    public class UpdateArchivoRequestDto
    {
        public string ImageBase64 { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
        public string Description { get; set; } = string.Empty;
        public string ExtensionImage { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string CreatedRoute { get; set; } = string.Empty;
    }
}
