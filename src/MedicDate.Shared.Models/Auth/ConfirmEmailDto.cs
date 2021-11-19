namespace MedicDate.Shared.Models.Auth;

public class ConfirmEmailDto
{
  public string UserId { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;
}