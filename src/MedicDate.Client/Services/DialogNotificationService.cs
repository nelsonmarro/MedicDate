using MedicDate.Client.Components;
using MedicDate.Client.Services.IServices;
using Radzen;

namespace MedicDate.Client.Services
{
    public class DialogNotificationService : IDialogNotificationService
    {
        private readonly DialogService _dialogService;

        public DialogNotificationService(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task ShowSuccess(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    { "Icon", "check_circle_outline" },
                    { "Title", title },
                    { "Text", details },
                    { "Color", "success" }
                });
        }

        public async Task ShowError(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    { "Icon", "error" },
                    { "Title", title },
                    { "Text", details },
                    { "Color", "danger" }
                });
        }

        public async Task ShowWarning(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    { "Icon", "warning" },
                    { "Title", title },
                    { "Text", details },
                    { "Color", "warning" }
                });
        }

        public async Task ShowInfo(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    { "Icon", "info" },
                    { "Title", title },
                    { "Text", details },
                    { "Color", "info" }
                });
        }
    }
}