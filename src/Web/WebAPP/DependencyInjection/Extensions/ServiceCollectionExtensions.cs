﻿using CorrelationId.HttpClient;
using Microsoft.Extensions.Options;
using Polly;
using Refit;
using WebAPP.DependencyInjection.HttpPolicies;
using WebAPP.DependencyInjection.Options;
using WebAPP.Store.Cataloging.Commands;
using WebAPP.Store.Cataloging.Queries;
using WebAPP.Store.Catalogs.Queries;

namespace WebAPP.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApis(this IServiceCollection services)
    {
        services.AddApiClient<ICreateCatalogApi>();
        services.AddApiClient<IChangeTitleApi>();
        services.AddApiClient<IChangeDescriptionApi>();
        services.AddApiClient<IDeleteCatalogApi>();
        services.AddApiClient<IListCatalogsApi>();
        services.AddApiClient<IListCatalogItemsApi>();
        services.AddApiClient<IAddCatalogItemApi>();
        services.AddApiClient<ISearchProductsApi>();

        // TODO: It is necessary given the incompetence of the Refit team. Remove when the issue is fixed. 
        services.AddScoped<HttpRequestExceptionHandler>();
    }

    private static void AddApiClient<TApi>(this IServiceCollection services) where TApi : class
    {
        services
            .AddRefitClient<TApi>()
            .AddCorrelationIdForwarding()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ECommerceHttpClientOptions>>().Value;
                client.BaseAddress = new(options.BaseAddress);
                client.Timeout = options.OverallTimeout;
            })
            .AddPolicyHandler((provider, _) =>
            {
                var options = provider.GetRequiredService<IOptions<ECommerceHttpClientOptions>>().Value;

                return Policy.WrapAsync(
                    HttpPolicy.GetRetryPolicyAsync(options.RetryCount, options.SleepDurationPower,
                        options.EachRetryTimeout),
                    HttpPolicy.GetCircuitBreakerPolicyAsync(options.CircuitBreaking, options.DurationOfBreak));
            })
            // TODO: It is necessary given the incompetence of the Refit team. Remove when the issue is fixed. 
            .AddHttpMessageHandler<HttpRequestExceptionHandler>();
    }

    private static IServiceCollection AddLazyTransient<T>(this IServiceCollection services) where T : class
        => services.AddTransient<Lazy<T>>(provider => new(provider.GetRequiredService<T>));

    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
        => services.ConfigureOptions<ECommerceHttpClientOptions>();

    private static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
        => services
            .AddOptions<TOptions>()
            .BindConfiguration(typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services;
}