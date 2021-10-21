using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Archivo
{
    public class ArchivoRequestDto
    {
        public string RutaArchivo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }
}