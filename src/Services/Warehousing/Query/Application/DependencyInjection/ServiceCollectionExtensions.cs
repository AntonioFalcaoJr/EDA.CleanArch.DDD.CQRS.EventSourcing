using Application.Abstractions;
using Application.UseCases.Events;
using Application.UseCases.Queries;
using Contracts.Boundaries.Warehouse.Inventory;
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
            .AddScoped<IProjectInventoryGridItemWhenInventoryChangedInteractor, ProjectInventoryGridItemWhenInventoryChangedInteractor>()
            .AddScoped<IProjectInventoryItemListItemWhenInventoryItemChangedInteractor, ProjectInventoryItemListItemWhenInventoryItemChangedInteractor>();

    private static IServiceCollection AddQueryInteractors(this IServiceCollection services)
        => services
            .AddScoped<IPagedInteractor<Query.ListInventoryGridItems, Projection.InventoryGridItem>, ListInventoriesGridItemInteractor>()
            .AddScoped<IPagedInteractor<Query.ListInventoryItemsListItems, Projection.InventoryItemListItem>, ListInventoryItemsListItemsInteractor>();
}