﻿using MassTransit;
using Serilog;

namespace Infrastructure.MessageBus.PipeObservers;

public class LoggingPublishObserver : IPublishObserver
{
    public async Task PrePublish<T>(PublishContext<T> context)
        where T : class
    {
        await Task.Yield();
        var messageType = context.Message.GetType();
        Log.Information("Publishing {Message} event from {Namespace}, CorrelationId: {CorrelationId}", messageType.Name, messageType.Namespace, context.CorrelationId);
    }

    public async Task PostPublish<T>(PublishContext<T> context)
        where T : class
    {
        await Task.Yield();
        var messageType = context.Message.GetType();
        Log.Debug("{MessageType} event, from {Namespace} was published, CorrelationId: {CorrelationId}", messageType.Name, messageType.Namespace, context.CorrelationId);
    }

    public async Task PublishFault<T>(PublishContext<T> context, Exception exception)
        where T : class
    {
        await Task.Yield();
        var messageType = context.Message.GetType();
        Log.Error("Fault on publishing message {Message} from {Namespace}, Error: {Error}, CorrelationId: {CorrelationId}", messageType.Name, messageType.Namespace ?? string.Empty, exception.Message, context.CorrelationId ?? new());
    }
}