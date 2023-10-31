namespace MedicDate.Bussines.Models;

public class RegisteredAppoimentEmailContent
{
  public string AppointmentDate { get; set; } = string.Empty;
  public string DoctorInfo { get; set; } = string.Empty;
  public string PacientInfo { get; set; } = string.Empty;
  public List<string> Treatments { get; set; } = new();
}

public class SendRegisteredEmailRequest
{
  public string AppoimentId { get; set; }
}
