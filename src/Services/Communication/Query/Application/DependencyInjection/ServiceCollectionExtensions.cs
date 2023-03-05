using Application.Abstractions;
using Application.UseCases.Events;
using Application.UseCases.Queries;
using Contracts.Services.Communication;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInteractors(this IServiceCollection services)
        => services
            .AddEventInteractors()
            .AddQueryInteractors();

    private static IServiceCollection AddEventInteractors(this IServiceCollection services)
        => services
            .AddScoped<IProjectNotificationDetailsWhenNotificationChangedInteractor, ProjectNotificationDetailsWhenNotificationChangedInteractor>();

    private static IServiceCollection AddQueryInteractors(this IServiceCollection services)
        => services
            .AddScoped<IPagedInteractor<Query.ListNotificationsDetails, Projection.NotificationDetails>, ListNotificationsDetailsInteractor>();
}