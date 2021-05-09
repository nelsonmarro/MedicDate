using System.Threading.Tasks;
using MedicDate.Client.Helpers;

namespace MedicDate.Client.Data.HttpRepository.IHttpRepository
{
    public interface IHttpRepository
    {
        public Task<HttpResponseWrapper<T>> Get<T>(string url);
        public Task<HttpResponseWrapper<object>> Post<T>(string url, T resource);
        public Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T resource);
        public Task<HttpResponseWrapper<object>> Put<T>(string url, T resource);
        public Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T resource);
        public Task<HttpResponseWrapper<object>> Delete(string url);
        public Task<HttpResponseWrapper<TResponse>> Delete<TResponse>(string url);
    }
}