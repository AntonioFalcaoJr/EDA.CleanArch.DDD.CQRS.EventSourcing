using Application.Abstractions;
using Application.Abstractions.Gateways;
using Infrastructure.EventStore.Contexts;
using Infrastructure.EventStore.DependencyInjection.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.EventStore.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEventStoreInfrastructure(this IServiceCollection services)
        => services
            .ConfigureOptions()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IEventStoreGateway, EventStoreGateway>()
            .AddDbContextPool<DbContext, EventStoreDbContext>((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptions<SqlServerRetryOptions>>().Value;

                builder
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(
                        connectionString: configuration.GetConnectionString("EventStore"),
                        sqlServerOptionsAction: optionsBuilder
                            => optionsBuilder.ExecutionStrategy(
                                    dependencies => new SqlServerRetryingExecutionStrategy(
                                        dependencies: dependencies,
                                        maxRetryCount: options.MaxRetryCount,
                                        maxRetryDelay: options.MaxRetryDelay,
                                        errorNumbersToAdd: options.ErrorNumbersToAdd))
                                .MigrationsAssembly(typeof(EventStoreDbContext).Assembly.GetName().Name));
            });

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
        => services
            .ConfigureOptions<SqlServerRetryOptions>()
            .ConfigureOptions<EventStoreOptions>();

    private static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
        => services
            .AddOptionsWithValidateOnStart<TOptions>()
            .BindConfiguration(typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .Services;
}