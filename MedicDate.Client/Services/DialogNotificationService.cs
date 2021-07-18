﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
                    {"Icon", "fa-check"},
                    {"Title", title},
                    {"Text", details},
                    {"Color", "success"}
                },
                new DialogOptions()
                {
                    Width = "485px", Height = "340px"
                });
        }

        public async Task ShowError(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    {"Icon", "fa-times-circle"},
                    {"Title", title},
                    {"Text", details},
                    {"Color", "danger"}
                },
                new DialogOptions()
                {
                    Width = "485px", Height = "340px"
                });
        }

        public async Task ShowWarning(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    {"Icon", "fa-exclamation-triangle"},
                    {"Title", title},
                    {"Text", details},
                    {"Color", "warning"}
                },
                new DialogOptions()
                {
                    Width = "485px", Height = "340px"
                });
        }

        public async Task ShowInfo(string title, string details)
        {
            await _dialogService.OpenAsync<NotificationDialog>("",
                new Dictionary<string, object>()
                {
                    {"Icon", "fa-exclamation-circle"},
                    {"Title", title},
                    {"Text", details},
                    {"Color", "info"}
                },
                new DialogOptions()
                {
                    Width = "485px", Height = "340px"
                });
        }
    }
}