using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Auth;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace MedicDate.Client.Pages.Auth;

public partial class Login
{
    private readonly LoginRequestDto _loginRequestDto = new();

    private bool _showAuthError;

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; } = default!;

    [Inject]
    public INotificationService NotificationService { get; set; } = default!;

    [Inject] public DialogService DialogService { get; set; } = default!;

    public string Error { get; set; } = string.Empty;

    public async Task ExecuteLogin()
    {
        NotificationService.ShowLoadingDialog(DialogService);
        _showAuthError = false;

        var result = await AuthenticationService.Login(_loginRequestDto);

        NotificationService.CloseDialog(DialogService);

        if (result is null) return;
        if (!result.IsAuthSuccessful)
        {
            Error = result.ErrorMessage ?? "";
            _showAuthError = true;
        }
        else
        {
            NavigationManager.NavigateTo("/");
        }
    }
}