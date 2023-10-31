namespace MedicDate.Domain.Models;

public class AppoimentEmailContent
{
  public string AppointmentDate { get; set; } = string.Empty;
  public string DoctorInfo { get; set; } = string.Empty;
  public string PacientInfo { get; set; } = string.Empty;
  public List<string> Treatments { get; set; } = new();
}

public class SendRegisteredAppoimentEmailRequest
{
  public string AppoimentId { get; set; }
}
