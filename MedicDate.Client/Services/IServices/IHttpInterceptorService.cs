using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace MedicDate.Client.Services.IServices
{
    public interface IHttpInterceptorService
    {
        public void RegisterEvent();
        public Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);
        public void DisposeEvent();
    }
}