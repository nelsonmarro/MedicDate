using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors.IInterceptors
{
    public interface IErrorInterceptor : IInterceptor
    {
        public Task InterceptResponseErrorAsync(object sender, HttpClientInterceptorEventArgs e);
    }
}
