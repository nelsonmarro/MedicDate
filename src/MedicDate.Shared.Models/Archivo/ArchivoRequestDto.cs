using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Archivo
{
    public class ArchivoRequestDto
    {
        public string RutaArchivo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
        public string Descripcion { get; set; }
    }
}