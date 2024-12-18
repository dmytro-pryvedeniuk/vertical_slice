using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TravelInspiration.API.Shared.Behaviors;
using TravelInspiration.API.Shared.Metrics;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Security;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(currentAssembly);
        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(currentAssembly)
                .AddOpenRequestPreProcessor(typeof(LoggingBehavior<>))
                .AddOpenBehavior(typeof(ModelValidationBehavior<,>))
                .AddOpenBehavior(typeof(HandlerPerformanceMetricBehavior<,>));
        });
        services.RegisterSlices();
        services.AddSingleton<HandlerPerformanceMetric>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TravelInspirationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("TravelInspirationDbConnection");
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}
