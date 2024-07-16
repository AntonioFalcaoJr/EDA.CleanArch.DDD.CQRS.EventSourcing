using System.Reflection;
using Application.Abstractions;
using Contracts.JsonConverters;
using Infrastructure.EventBus.DependencyInjection.Options;
using Infrastructure.EventBus.PipeFilters;
using Infrastructure.EventBus.PipeObservers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.EventBus.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    private const string SchedulerQueueName = "event-scheduler";

    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
        => services
            .ConfigureOptions()
            .AddEventBusGateway()
            .AddQuartz()
            .AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddConsumers(Assembly.GetExecutingAssembly());
                cfg.AddMessageScheduler(new($"queue:{SchedulerQueueName}"));

                cfg.UsingRabbitMq((context, bus) =>
                {
                    var options = context.GetRequiredService<IOptionsMonitor<EventBusOptions>>().CurrentValue;

                    bus.Host(
                        hostAddress: options.ConnectionString,
                        connectionName: $"{options.ConnectionName}.{AppDomain.CurrentDomain.FriendlyName}");

                    bus.UseInMemoryScheduler(
                        schedulerFactory: context.GetRequiredService<ISchedulerFactory>(),
                        queueName: SchedulerQueueName);

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

                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                    bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                    bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                    bus.ConnectPublishObserver(new LoggingPublishObserver());
                    bus.ConnectSendObserver(new LoggingSendObserver());

                    bus.UsePublishFilter(typeof(TraceIdentifierFilter<>), context);

                    bus.ConfigureEventReceiveEndpoints(context);
                    bus.ConfigureEndpoints(context);
                });
            });

    private static IServiceCollection AddEventBusGateway(this IServiceCollection services)
        => services.AddScoped<IEventBusGateway, EventBusGateway>();

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
        => services
            .ConfigureOptions<EventBusOptions>()
            .ConfigureOptions<QuartzOptions>()
            .ConfigureOptions<MassTransitHostOptions>();

    private static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
        => services
            .AddOptionsWithValidateOnStart<TOptions>()
            .BindConfiguration(typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .Services;
}