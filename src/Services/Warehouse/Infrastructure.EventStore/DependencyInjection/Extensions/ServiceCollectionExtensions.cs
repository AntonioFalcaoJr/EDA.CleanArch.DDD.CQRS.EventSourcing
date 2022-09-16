﻿using Application.EventStore;
using Infrastructure.EventStore.Contexts;
using Infrastructure.EventStore.DependencyInjection.Options;
using Infrastructure.EventStore.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.EventStore.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEventStore(this IServiceCollection services)
    {
        services.AddScoped<IWarehouseEventStoreService, WarehouseEventStoreService>();
        services.AddScoped<IWarehouseEventStoreRepository, WarehouseEventStoreRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContextPool<EventStoreDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsSnapshot<SqlServerRetryOptions>>();

            builder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString("EventStore"),
                    sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.Value.MaxRetryCount,
                                    maxRetryDelay: options.Value.MaxRetryDelay,
                                    errorNumbersToAdd: options.Value.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(EventStoreDbContext).Assembly.GetName().Name));
        });
    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<SqlServerRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static OptionsBuilder<EventStoreOptions> ConfigureEventStoreOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<EventStoreOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}