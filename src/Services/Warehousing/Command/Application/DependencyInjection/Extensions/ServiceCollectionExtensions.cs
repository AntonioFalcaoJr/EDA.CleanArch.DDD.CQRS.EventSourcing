using Application.Abstractions;
using Application.Services;
using Application.UseCases.Commands;
using Application.UseCases.Events;
using Contracts.Boundaries.Warehouse.Inventory;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services
            .AddScoped<IApplicationService, ApplicationService>();

    public static IServiceCollection AddCommandInteractors(this IServiceCollection services)
        => services
            .AddScoped<IInteractor<Command.CreateInventory>, CreateInventoryInteractor>()
            .AddScoped<IInteractor<Command.DecreaseInventoryAdjust>, DecreaseInventoryAdjustInteractor>()
            .AddScoped<IInteractor<Command.IncreaseInventoryAdjust>, IncreaseInventoryAdjustInteractor>()
            .AddScoped<IInteractor<Command.ReceiveInventoryItem>, ReceiveInventoryItemInteractor>();

    public static IServiceCollection AddEventInteractors(this IServiceCollection services)
        => services
            .AddScoped<IReserveInventoryItemWhenCartItemAddedInteractor, ReserveInventoryItemWhenCartItemAddedInteractor>();
}