using Microsoft.Extensions.DependencyInjection;

namespace MedicDate.Infrastructure.Services;

public class ServiceActivator
{
  private static IServiceProvider? _serviceProvider;

  public static void Configure(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public static IServiceScope GetScope(IServiceProvider? serviceProvider = null)
  {
    var provider = serviceProvider ?? _serviceProvider;
    var scope = provider?.GetRequiredService<IServiceScopeFactory>()
      .CreateScope();

    if (scope != null) return scope;

    throw new NotSupportedException("No se pudo obtener el servicio requerido");
  }
}