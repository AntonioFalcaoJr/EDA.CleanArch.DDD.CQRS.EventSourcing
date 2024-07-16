using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Payment;

public static class Projection
{
    public record PaymentDetails(string Id, Guid OrderId, Dto.Money Amount, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Payment.Protobuf.PaymentDetails(PaymentDetails payment)
            => new()
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId.ToString(),
                Amount = payment.Amount,
                Status = payment.Status
            };
    }

    public record PaymentMethodDetails(string Id, Guid PaymentId, Guid OrderId, Dto.Money Amount, Dto.IPaymentOption Option, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Payment.Protobuf.PaymentMethodDetails(PaymentMethodDetails method)
            => new()
            {
                MethodId = method.Id,
                PaymentId = method.PaymentId.ToString(),
                OrderId = method.OrderId.ToString(),
                Amount = method.Amount,
                Status = method.Status,
                Option = method.Option switch
                {
                    Dto.CreditCard creditCard => new() { CreditCard = creditCard },
                    Dto.DebitCard debitCard => new() { DebitCard = debitCard },
                    Dto.PayPal payPal => new() { PayPal = payPal },
                    _ => default
                }
            };
    }

    public record PaymentMethodListItem(string Id, Guid OrderId, Dto.Money Amount, string Option, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Payment.Protobuf.PaymentMethodListItem(PaymentMethodListItem method)
            => new()
            {
                MethodId = method.Id,
                OrderId = method.OrderId.ToString(),
                Amount = method.Amount,
                Status = method.Status,
                Option = method.Option
            };
    }
}