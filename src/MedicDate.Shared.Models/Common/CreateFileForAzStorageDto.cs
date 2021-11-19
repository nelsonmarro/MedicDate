namespace MedicDate.Shared.Models.Common;

public class CreateFileForAzStorageDto
{
  public byte[] Content { get; set; } = Array.Empty<byte>();
  public string Extension { get; set; } = string.Empty;
  public string Container { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
}