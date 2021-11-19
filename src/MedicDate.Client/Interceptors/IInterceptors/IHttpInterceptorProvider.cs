namespace MedicDate.Client.Interceptors.IInterceptors;

public interface IHttpInterceptorProvider
{
  public IErrorInterceptor ErrorInterceptor { get; }
  public IAuthTokenInterceptor AuthTokenInterceptor { get; }
}