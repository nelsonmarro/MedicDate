using MedicDate.Shared.Models.Common;

namespace MedicDate.Client.ViewModels;

public class ArchivoCitaVm
{
  public string Id { get; set; } = string.Empty;
  public string RutaArchivo { get; set; } = string.Empty;
  public Base64ImgDto ImageInfo { get; set; } = new();
  public string Descripcion { get; set; } = string.Empty;
  public string Extension { get; set; } = string.Empty;
}