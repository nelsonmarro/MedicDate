using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Auth
{
    public partial class Logout : ComponentBase
    {
        [Inject] public IAuthenticationService AuthenticationService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/login");
        }
    }
}
