using System.Threading.Tasks;
using MedicDate.Client.Services.IServices;
using MedicDate.Client.Shared;
using Radzen;

namespace MedicDate.Client.Services
{
    public class RadzenNotificationService : INotificationService
    {
        private readonly NotificationService _notificationService;

        public RadzenNotificationService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ShowSuccess(string title, string details)
        {
            _notificationService.Notify(new NotificationMessage
                {Severity = NotificationSeverity.Success, Summary = title, Detail = details, Duration = 4600});
        }

        public void ShowError(string title, string details)
        {
            _notificationService.Notify(new NotificationMessage
                {Severity = NotificationSeverity.Error, Summary = title, Detail = details, Duration = 4600});
        }

        public void ShowWarning(string title, string details)
        {
            _notificationService.Notify(new NotificationMessage
                {Severity = NotificationSeverity.Warning, Summary = title, Detail = details, Duration = 4600});
        }

        public void ShowInfo(string title, string details)
        {
            _notificationService.Notify(new NotificationMessage
                {Severity = NotificationSeverity.Info, Summary = title, Detail = details, Duration = 4600});
        }

        public void ShowLoadingDialog(DialogService dialogService)
        {
            dialogService.Open<LoadingDialog>("", null,
                new DialogOptions {ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto"});
        }

        public void CloseDialog(DialogService dialogService)
        {
            dialogService.Close();
        }
    }
}