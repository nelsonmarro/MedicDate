using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedicDate.Client.Data.HttpRepository
{
    public class HttpRepository : IHttpRepository.IHttpRepository
    {
        private readonly HttpClient _http;
        private readonly INotificationService _notificationService;

        private readonly JsonSerializerOptions _defaultSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpRepository(HttpClient http, INotificationService notificationService)
        {
            _http = http;
            _notificationService = notificationService;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            try
            {
                var response = await _http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var deserializedResp = await DeserializeResponse<T>(response, _defaultSerializerOptions);
                    return new HttpResponseWrapper<T>(deserializedResp, false, response);
                }

                return new HttpResponseWrapper<T>(default, true, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T resource)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, resource);
                return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T resource)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, resource);

                if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

                var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

                return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T resource)
        {
            try
            {
                var response = await _http.PutAsJsonAsync(url, resource);
                if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

                var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

                return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T resource)
        {
            try
            {
                var response = await _http.PutAsJsonAsync(url, resource);
                return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            try
            {
                var response = await _http.DeleteAsync(url);
                return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> Delete<TResponse>(string url)
        {
            try
            {
                var response = await _http.DeleteAsync(url);
                if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

                var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

                return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notificationService.ShowError("Error!", "Error al enviar la petición al servidor");
                return null;
            }
        }

        private static async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions
                jsonSerializerOptions)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
        }
    }
}