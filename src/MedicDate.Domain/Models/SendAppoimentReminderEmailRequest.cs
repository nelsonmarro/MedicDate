using MedicDate.DataAccess.Entities;

namespace MedicDate.Domain.Models
{
  public class SendAppoimentReminderEmailRequest
  {
    public Cita Cita { get; set; }
  }
}
