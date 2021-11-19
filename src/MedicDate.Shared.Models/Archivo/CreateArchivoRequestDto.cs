using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Archivo;

public class CreateArchivoRequestDto
{
  public string? CitaId { get; set; }
  public string ImageBase64 { get; set; } = string.Empty;

  [StringLength(500,
    ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
  public string Descripcion { get; set; } = string.Empty;

  public string ExtensionImage { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
}