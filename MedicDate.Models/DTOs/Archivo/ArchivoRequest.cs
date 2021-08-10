using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Archivo
{
    public class ArchivoRequest
    {
        public string RutaArchivo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
        public string Descripcion { get; set; }
    }
}