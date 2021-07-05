using Blazored.LocalStorage;
using MedicDate.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using System;
using MedicDate.Utility;

namespace MedicDate.Client.Services
{
    public class AuthenticationService : HttpRepository, IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient http, ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider, INotificationService notificationService) : base(http, notificationService)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var response = await Post<LoginRequest, LoginResponse>("api/Account/login", loginRequest);

            if (response is null)
            {
                return new LoginResponse();
            }

            if (response.Error)
            {
                return await response.HttpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            await _localStorage.SetItemAsync(Sd.TOKEN_ACCESS, response.Response.Token);
            await _localStorage.SetItemAsync(Sd.TOKEN_REFRESH, response.Response.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyLogin(response.Response.Token);

            return response.Response;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(Sd.TOKEN_ACCESS);
            await _localStorage.RemoveItemAsync(Sd.TOKEN_REFRESH);
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>(Sd.TOKEN_ACCESS);
            var refreshToken = await _localStorage.GetItemAsync<string>(Sd.TOKEN_REFRESH);

            var refreshResult = await Post<RefreshTokenDto, LoginResponse>("api/Token/refresh",
                new RefreshTokenDto() { Token = token, RefreshToken = refreshToken });

            if (refreshResult.Error)
            {
                throw new ApplicationException("Algo falló durante la renovación del token");
            }

            await _localStorage.SetItemAsync(Sd.TOKEN_ACCESS, refreshResult.Response.Token);
            await _localStorage.SetItemAsync(Sd.TOKEN_REFRESH, refreshResult.Response.RefreshToken);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", refreshResult.Response.Token);

            return refreshResult.Response.Token;
        }
    }
}