using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicDate.Shared.Models.Archivo;

public class UpdateArchivoRequestDto
{
  [JsonIgnore] public string Id { get; set; } = string.Empty;

  public string ImageBase64 { get; set; } = string.Empty;

  [StringLength(500,
    ErrorMessage = "La descripción debe tener un máximo de {1} caracteres")]
  public string Description { get; set; } = string.Empty;

  public string ExtensionImage { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public string CreatedRoute { get; set; } = string.Empty;
}