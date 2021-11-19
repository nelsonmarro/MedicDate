namespace MedicDate.Client.Services.IServices;

public interface IDialogNotificationService
{
  Task ShowSuccess(string title, string details);
  Task ShowError(string title, string details);
  Task ShowWarning(string title, string details);
  Task ShowInfo(string title, string details);
}