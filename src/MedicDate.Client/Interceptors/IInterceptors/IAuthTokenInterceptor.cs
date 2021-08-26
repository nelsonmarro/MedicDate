using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors.IInterceptors
{
    public interface IAuthTokenInterceptor : IInterceptor
    {
        public Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);
    }
}