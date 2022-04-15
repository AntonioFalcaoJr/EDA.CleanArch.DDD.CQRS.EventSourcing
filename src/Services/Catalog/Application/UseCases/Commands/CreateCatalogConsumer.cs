﻿using Application.EventSourcing.EventStore;
using Domain.Aggregates;
using MassTransit;
using CreateCatalogCommand = ECommerce.Contracts.Catalog.Commands.CreateCatalog;

namespace Application.UseCases.Commands;

public class CreateCatalogConsumer : IConsumer<CreateCatalogCommand>
{
    private readonly ICatalogEventStoreService _eventStoreService;

    public CreateCatalogConsumer(ICatalogEventStoreService eventStoreService)
    {
        _eventStoreService = eventStoreService;
    }

    public async Task Consume(ConsumeContext<CreateCatalogCommand> context)
    {
        var catalog = new Catalog();
        catalog.Handle(context.Message);
        await _eventStoreService.AppendEventsToStreamAsync(catalog, context.CancellationToken);
    }
}