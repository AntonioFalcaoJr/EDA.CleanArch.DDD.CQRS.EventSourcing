using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Order;

public static class DomainEvent
{
    public record OrderPlaced(Guid OrderId, Guid CustomerId, Dto.Money Total, Dto.Address BillingAddress, Dto.Address ShippingAddress, IEnumerable<Dto.OrderItem> Items,
        IEnumerable<Dto.PaymentMethod> PaymentMethods, string Status, ulong Version) : Message, IDomainEvent;

    public record OrderConfirmed(Guid OrderId, string Status, ulong Version) : Message, IDomainEvent;
}