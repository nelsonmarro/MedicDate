using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicDate.Shared.Models.Cita;
public class CitaEmailContent
{
  public string AppointmentDate { get; set; } = string.Empty;
  public string DoctorInfo { get; set; } = string.Empty;
  public string PatientInfo { get; set; } = string.Empty;
  public List<string> Treatments { get; set; } = new();
}
