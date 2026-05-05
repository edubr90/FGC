using FGC.IoC.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FGC.IoC;
public static class DependencyInjection
{
    public static IServiceCollection AddFcgServices(this IServiceCollection services, IConfiguration config)
    {
        services
        .AddApplicationModule()
        .AddInfrastructureModule(config)
        .AddAuthenticationModule(config)
        .AddSwaggerModule();

        return services;
    }
}
