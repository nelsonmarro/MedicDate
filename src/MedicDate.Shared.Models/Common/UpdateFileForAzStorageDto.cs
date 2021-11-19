namespace MedicDate.Shared.Models.Common;

public class UpdateFileForAzStorageDto : CreateFileForAzStorageDto
{
  public string CreatedRoute { get; set; } = string.Empty;
}