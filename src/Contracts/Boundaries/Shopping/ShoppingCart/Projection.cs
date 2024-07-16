using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Shopping.ShoppingCart;

public static class Projection
{
    public record ShoppingCartDetails(string Id, Guid CustomerId, Dto.Money Total, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Contracts.Shopping.Queries.ShoppingCartDetails(ShoppingCartDetails cart)
            => new()
            {
                CartId = cart.Id,
                CustomerId = cart.CustomerId.ToString(),
                Status = cart.Status,
                Total = cart.Total
            };
    }

    public record ShoppingCartItemDetails(string Id, Guid CartId, Dto.Product Product, int Quantity, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Contracts.Shopping.Queries.ShoppingCartItemDetails(ShoppingCartItemDetails item)
            => new()
            {
                ItemId = item.Id,
                CartId = item.CartId.ToString(),
                Product = item.Product,
                Quantity = item.Quantity
            };
    }

    public record PaymentMethodDetails(string Id, Guid CartId, Dto.Money Amount, Dto.IPaymentOption Option, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Contracts.Shopping.Queries.PaymentMethodDetails(PaymentMethodDetails method)
            => new()
            {
                MethodId = method.Id,
                CartId = method.CartId.ToString(),
                Amount = method.Amount,
                Option = method.Option switch
                {
                    Dto.CreditCard creditCard => new() { CreditCard = creditCard },
                    Dto.DebitCard debitCard => new() { DebitCard = debitCard },
                    Dto.PayPal payPal => new() { PayPal = payPal },
                    _ => default
                }
            };
    }

    public record ShoppingCartItemListItem(string Id, Guid CartId, string ProductName, int Quantity, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Contracts.Shopping.Queries.ShoppingCartItemListItem(ShoppingCartItemListItem item)
            => new()
            {
                ItemId = item.Id,
                CartId = item.CartId.ToString(),
                ProductName = item.ProductName,
                Quantity = item.Quantity
            };
    }

    public record PaymentMethodListItem(string Id, Guid CartId, Dto.Money Amount, string Option, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Contracts.Shopping.Queries.PaymentMethodListItem(PaymentMethodListItem method)
            => new()
            {
                MethodId = method.Id,
                CartId = method.CartId.ToString(),
                Amount = method.Amount,
                Option = method.Option
            };
    }
}