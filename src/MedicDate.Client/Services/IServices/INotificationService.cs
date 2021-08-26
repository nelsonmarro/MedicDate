using Radzen;
using System;
using System.Threading.Tasks;

namespace MedicDate.Client.Services.IServices
{
    public interface INotificationService
    {
        void ShowSuccess(string title, string details);
        void ShowError(string title, string details);
        void ShowWarning(string title, string details);
        void ShowInfo(string title, string details);
        void ShowLoadingDialog(DialogService dialogService);
        void CloseDialog(DialogService dialogService);
    }
}