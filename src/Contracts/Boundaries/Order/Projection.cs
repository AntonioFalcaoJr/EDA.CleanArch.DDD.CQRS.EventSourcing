using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Order;

public static class Projection
{
    public record OrderDetails(string Id, Guid CustomerId, Dto.Money Total, Dto.Address BillingAddress, Dto.Address ShippingAddress, IEnumerable<Dto.OrderItem> Items,
        IEnumerable<Dto.PaymentMethod> PaymentMethods, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Order.Protobuf.OrderDetails(OrderDetails order)
            => new()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId.ToString(),
                Total = order.Total,
                Status = order.Status
            };
    }

    public record OrderGridItem(string Id, Guid CustomerId, Dto.Money Total, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Order.Protobuf.OrderGridItem(OrderGridItem order)
            => new()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId.ToString(),
                Total = order.Total,
                Status = order.Status
            };
    }
}