﻿using Application.Services;
using Application.Services.CreditCards;
using Application.Services.CreditCards.Http;
using Application.Services.DebitCards;
using Application.Services.DebitCards.Http;
using Application.Services.PayPal;
using Application.Services.PayPal.Http;
using Infrastructure.HttpClients.CreditCards;
using Infrastructure.HttpClients.DebitCards;
using Infrastructure.HttpClients.DependencyInjection.HttpPolicies;
using Infrastructure.HttpClients.DependencyInjection.Options;
using Infrastructure.HttpClients.PayPals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;

namespace Infrastructure.HttpClients.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPaymentGateway(this IServiceCollection services)
        => services.AddScoped<IPaymentGateway, PaymentGateway>();

    public static void AddPayPalHttpClient(this IServiceCollection services)
    {
        services.AddScoped<IPayPalPaymentService, PayPalPaymentService>();

        services
            .AddHttpClient<IPayPalHttpClient, PayPalHttpClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<PayPalHttpClientOptions>>().Value;
                client.BaseAddress = new(options.BaseAddress);
                client.Timeout = options.OverallTimeout;
            })
            .AddPolicyHandler((provider, _) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<PayPalHttpClientOptions>>().Value;

                return Policy.WrapAsync(
                    HttpPolicy.GetRetryPolicyAsync(options.RetryCount, options.SleepDurationPower, options.EachRetryTimeout),
                    HttpPolicy.GetCircuitBreakerPolicyAsync(options.CircuitBreaking, options.DurationOfBreak));
            });
    }

    public static void AddCreditCardHttpClient(this IServiceCollection services)
    {
        services.AddScoped<ICreditCardPaymentService, CreditCardPaymentService>();

        services
            .AddHttpClient<ICreditCardHttpClient, CreditCardHttpClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<CreditCardHttpClientOptions>>().Value;
                client.BaseAddress = new(options.BaseAddress);
                client.Timeout = options.OverallTimeout;
            })
            .AddPolicyHandler((provider, _) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<CreditCardHttpClientOptions>>().Value;

                return Policy.WrapAsync(
                    HttpPolicy.GetRetryPolicyAsync(options.RetryCount, options.SleepDurationPower, options.EachRetryTimeout),
                    HttpPolicy.GetCircuitBreakerPolicyAsync(options.CircuitBreaking, options.DurationOfBreak));
            });
    }

    public static void AddDebitCardHttpClient(this IServiceCollection services)
    {
        services.AddScoped<IDebitCardPaymentService, DebitCardPaymentService>();

        services
            .AddHttpClient<IDebitCardHttpClient, DebitCardHttpClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<DebitCardHttpClientOptions>>().Value;
                client.BaseAddress = new(options.BaseAddress);
                client.Timeout = options.OverallTimeout;
            })
            .AddPolicyHandler((provider, _) =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<DebitCardHttpClientOptions>>().Value;

                return Policy.WrapAsync(
                    HttpPolicy.GetRetryPolicyAsync(options.RetryCount, options.SleepDurationPower, options.EachRetryTimeout),
                    HttpPolicy.GetCircuitBreakerPolicyAsync(options.CircuitBreaking, options.DurationOfBreak));
            });
    }

    public static OptionsBuilder<PayPalHttpClientOptions> ConfigurePayPalHttpClientOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<PayPalHttpClientOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static OptionsBuilder<CreditCardHttpClientOptions> ConfigureCreditCardHttpClientOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<CreditCardHttpClientOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static OptionsBuilder<DebitCardHttpClientOptions> ConfigureDebitCardHttpClientOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<DebitCardHttpClientOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}