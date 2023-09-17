using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using MedicDate.Client.Auth;
using MedicDate.Client.Data.HttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Auth;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MedicDate.Client.Services
{
    public class AuthenticationService : HttpRepository, IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly INotificationService _notificationService;
        private readonly NavigationManager _navigationManager;

        public AuthenticationService(HttpClient http, ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider, INotificationService notificationService,
            NavigationManager navigationManager) : base(http)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _notificationService = notificationService;
            _navigationManager = navigationManager;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var response = await Post<LoginRequestDto, LoginResponseDto>("api/Account/login", loginRequestDto);

            if (response.Error) return await response.HttpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDto>();

            await _localStorage.SetItemAsync(Sd.TOKEN_ACCESS, response.Response?.Token);
            await _localStorage.SetItemAsync(Sd.TOKEN_REFRESH, response.Response?.RefreshToken);

            ((AuthStateProvider) _authStateProvider).NotifyLogin(response.Response?.Token ?? "");

            return new LoginResponseDto { IsAuthSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(Sd.TOKEN_ACCESS);
            await _localStorage.RemoveItemAsync(Sd.TOKEN_REFRESH);
            ((AuthStateProvider) _authStateProvider).NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string?> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>(Sd.TOKEN_ACCESS);
            var refreshToken = await _localStorage.GetItemAsync<string>(Sd.TOKEN_REFRESH);

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                ((AuthStateProvider) _authStateProvider).NotifyUserLogout();
                _http.DefaultRequestHeaders.Authorization = null;

                _notificationService.ShowInfo("Info!", "No se pudo recuperar las credenciales del usuario. Inicie Sesión otra vez");
                _navigationManager.NavigateTo("logout");

                return "";
            }

            var refreshResult = await Post<RefreshTokenDto, LoginResponseDto>("api/Token/refresh",
                new RefreshTokenDto() { Token = token, RefreshToken = refreshToken });

            if (refreshResult.Error)
            {
                await _localStorage.RemoveItemAsync(Sd.TOKEN_ACCESS);
                await _localStorage.RemoveItemAsync(Sd.TOKEN_REFRESH);
                ((AuthStateProvider) _authStateProvider).NotifyUserLogout();
                _http.DefaultRequestHeaders.Authorization = null;

                _notificationService.ShowError("Error!", "Error al validar las credenciales del usuario");
                _navigationManager.NavigateTo("logout");

                return "";
            }

            if (refreshResult.Response?.Token is null)
            {
                await _localStorage.RemoveItemAsync(Sd.TOKEN_ACCESS);
                await _localStorage.RemoveItemAsync(Sd.TOKEN_REFRESH);
                ((AuthStateProvider) _authStateProvider).NotifyUserLogout();
                _http.DefaultRequestHeaders.Authorization = null;

                _notificationService.ShowError("Error!", "Error al validar las credenciales del usuario");
                _navigationManager.NavigateTo("logout");

                return "";
            }

            await _localStorage.SetItemAsync(Sd.TOKEN_ACCESS, refreshResult.Response.Token);
            await _localStorage.SetItemAsync(Sd.TOKEN_REFRESH, refreshResult.Response.RefreshToken);

            _http.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue("bearer", refreshResult.Response.Token);

            return refreshResult.Response?.Token;
        }
    }
}