using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components.Authorization;

namespace MedicDate.Client.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
   private readonly AuthenticationState _anonymous;
   private readonly HttpClient _httpClient;
   private readonly IHttpRepository _httpRepo;
   private readonly ILocalStorageService _localStorage;

   public AuthStateProvider(HttpClient httpClient, IHttpRepository httpRepo,
      ILocalStorageService localStorage)
   {
      _httpClient = httpClient;
      _httpRepo = httpRepo;
      _localStorage = localStorage;
      _anonymous =
         new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
   }

   public override async Task<AuthenticationState> GetAuthenticationStateAsync()
   {
      var token = await _localStorage.GetItemAsync<string>(Sd.TOKEN_ACCESS);

      return string.IsNullOrWhiteSpace(token)
         ? _anonymous
         : BuildAuthenticationState(token);
   }

   private AuthenticationState BuildAuthenticationState(string token)
   {
      _httpClient.DefaultRequestHeaders.Authorization =
         new AuthenticationHeaderValue("bearer", token);

      return new AuthenticationState(
         new ClaimsPrincipal(
            new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
               Sd.AUTH_TYPE_JWT)));
   }

   public void NotifyLogin(string token)
   {
      var authState = BuildAuthenticationState(token);
      NotifyAuthenticationStateChanged(Task.FromResult(authState));
   }

   public void NotifyUserLogout()
   {
      var authState = Task.FromResult(_anonymous);
      NotifyAuthenticationStateChanged(authState);
   }
}