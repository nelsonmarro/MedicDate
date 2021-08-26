using System.Threading.Tasks;
using MedicDate.Client.Helpers;

namespace MedicDate.Client.Data.HttpRepository.IHttpRepository
{
    public interface IHttpRepository
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T resource);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T resource);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T resource);
        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T resource);
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<TResponse>> Delete<TResponse>(string url);
    }
}