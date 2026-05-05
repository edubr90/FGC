using FCG.Application.Interfaces;
using FCG.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FGC.IoC.Modules;
public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGameService, GameService>();
        return services;
    }
}
