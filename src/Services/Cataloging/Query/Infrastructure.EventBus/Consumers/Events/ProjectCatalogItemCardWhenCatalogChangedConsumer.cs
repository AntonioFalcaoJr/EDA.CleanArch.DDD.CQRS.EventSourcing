using Application.UseCases.Events;
using Infrastructure.EventBus.Abstractions;

namespace Infrastructure.EventBus.Consumers.Events;

public class ProjectCatalogItemCardWhenCatalogChangedConsumer(IProjectCatalogItemCardWhenCatalogChangedInteractor interactor)
    : Consumer<DomainEvent.CatalogItemAdded>(interactor);