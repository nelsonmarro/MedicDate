using MedicDate.Client.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedicDate.Client.Data.HttpRepository
{
    public class HttpRepository : IHttpRepository.IHttpRepository
    {
        private readonly HttpClient _http;

        private readonly JsonSerializerOptions _defaultSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpRepository(HttpClient _http)
        {
            this._http = _http;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var deserializedResp = await DeserializeResponse<T>(response, _defaultSerializerOptions);
                return new HttpResponseWrapper<T>(deserializedResp, false, response);
            }

            return new HttpResponseWrapper<T>(default, true, response);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T resource)
        {
            var response = await _http.PostAsJsonAsync(url, resource);
            return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T resource)
        {
            var response = await _http.PostAsJsonAsync(url, resource);

            if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

            var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

            return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T resource)
        {
            var response = await _http.PutAsJsonAsync(url, resource);
            if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

            var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

            return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T resource)
        {
            var response = await _http.PutAsJsonAsync(url, resource);
            return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var response = await _http.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Delete<TResponse>(string url)
        {
            var response = await _http.DeleteAsync(url);
            if (!response.IsSuccessStatusCode) return new HttpResponseWrapper<TResponse>(default, true, response);

            var deserializedResponse = await DeserializeResponse<TResponse>(response, _defaultSerializerOptions);

            return new HttpResponseWrapper<TResponse>(deserializedResponse, false, response);
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