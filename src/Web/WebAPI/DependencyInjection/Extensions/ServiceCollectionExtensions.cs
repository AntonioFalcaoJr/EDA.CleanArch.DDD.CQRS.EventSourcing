using Contracts.Abstractions.Messages;
using Contracts.JsonConverters;
using Contracts.Services.Account.Protobuf;
using Contracts.Services.Cataloging.Command.Protobuf;
using Contracts.Services.Communication.Protobuf;
using Contracts.Services.Identity.Protobuf;
using Contracts.Services.Payment.Protobuf;
using Contracts.Services.Warehouse.Protobuf;
using Contracts.Shopping.Commands;
using Contracts.Shopping.Queries;
using CorrelationId.Abstractions;
using CorrelationId.HttpClient;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client.Configuration;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPI.DependencyInjection.Options;
using WebAPI.PipeObservers;

namespace WebAPI.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwagger(this IServiceCollection services)
        => services
            .AddSwaggerGen()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    public static void AddEventBus(this IServiceCollection services)
        => services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, bus) =>
            {
                var options = context.GetRequiredService<IOptions<EventBusOptions>>().Value;

                bus.Host(options.ConnectionString);

                bus.UseMessageRetry(retry
                    => retry.Incremental(
                        retryLimit: options.RetryLimit,
                        initialInterval: options.InitialInterval,
                        intervalIncrement: options.IntervalIncrement));

                bus.UseNewtonsoftJsonSerializer();

                bus.ConfigureNewtonsoftJsonSerializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                bus.ConnectSendObserver(new LoggingSendObserver());
                bus.ConfigureEndpoints(context);

                bus.ConfigureSend(pipe => pipe.AddPipeSpecification(
                    new DelegatePipeSpecification<SendContext<ICommand>>(ctx =>
                    {
                        var accessor = context.GetRequiredService<ICorrelationContextAccessor>();
                        ctx.CorrelationId = new(accessor.CorrelationContext.CorrelationId);
                    })));
            });
        });

    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddScoped<GrpcExceptionInterceptor>();

        services.AddGrpcClient<IdentityService.IdentityServiceClient, IdentityGrpcClientOptions>();
        services.AddGrpcClient<AccountService.AccountServiceClient, AccountGrpcClientOptions>();
        services.AddGrpcClient<CommunicationService.CommunicationServiceClient, CommunicationGrpcClientOptions>();
        services.AddGrpcClient<WarehouseService.WarehouseServiceClient, WarehouseGrpcClientOptions>();
        services.AddGrpcClient<PaymentService.PaymentServiceClient, PaymentGrpcClientOptions>();

        // TODO: Improve and/or separate options for command and query clients
        services.AddGrpcClient<CatalogingCommandService.CatalogingCommandServiceClient, CatalogingCommandGrpcClientOptions>();
        services.AddGrpcClient<ShoppingQueryService.ShoppingQueryServiceClient, ShoppingCartGrpcClientOptions>();
        services.AddGrpcClient<ShoppingCommandService.ShoppingCommandServiceClient, ShoppingCartCommandGrpcClientOptions>();
    }

    public static void ConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<EventBusOptions>();
        services.ConfigureOptions<MassTransitHostOptions>();
        services.ConfigureOptions<RabbitMqTransportOptions>();
        services.ConfigureOptions<IdentityGrpcClientOptions>();
        services.ConfigureOptions<AccountGrpcClientOptions>();
        services.ConfigureOptions<CommunicationGrpcClientOptions>();
        services.ConfigureOptions<WarehouseGrpcClientOptions>();

        // TODO: Improve and/or separate options for command and query clients
        services.ConfigureOptions<CatalogingCommandGrpcClientOptions>();
        services.ConfigureOptions<CatalogingQueryGrpcClientOptions>();
        services.ConfigureOptions<ShoppingCartCommandGrpcClientOptions>();
        services.ConfigureOptions<ShoppingCartGrpcClientOptions>();

        services.ConfigureOptions<PaymentGrpcClientOptions>();
    }

    private static void AddGrpcClient<TClient, TOptions>(this IServiceCollection services)
        where TClient : ClientBase
        where TOptions : GrpcClientOptions
        => services.AddGrpcClient<TClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<TOptions>>().Value;
                client.Address = new(options.BaseAddress);
            })
            .AddInterceptor<GrpcExceptionInterceptor>()
            .ConfigureChannel(options =>
                {
                    options.Credentials = ChannelCredentials.Insecure;
                    options.ServiceConfig = new() { LoadBalancingConfigs = { new RoundRobinConfig() } };
                }
            )
            .ConfigurePrimaryHttpMessageHandler(() =>
                new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
                })
            .EnableCallContextPropagation(options => options.SuppressContextNotFoundErrors = true)
            .AddCorrelationIdForwarding();

    private static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
        => services
            .AddOptionsWithValidateOnStart<TOptions>()
            .BindConfiguration(typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .Services;
}

public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException ex)
        {
            throw;
        }
    }
}