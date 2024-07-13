using Contracts.Boundaries.Warehouse.Inventory;
using MassTransit;
using MediatR;

namespace Infrastructure.EventBus;

public class EventConsumer(ISender sender) : IConsumer<DomainEvent.InventoryItemReceived>
{
    public Task Consume(ConsumeContext<DomainEvent.InventoryItemReceived> context) 
        => sender.Send(context.Message);
}