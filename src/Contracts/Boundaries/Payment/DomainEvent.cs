using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Payment;

public static class DomainEvent
{
    public record PaymentRequested(Guid PaymentId, Guid OrderId, Dto.Money Amount, Dto.Address BillingAddress, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentCanceled(Guid PaymentId, Guid OrderId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentCompleted(Guid PaymentId, Guid OrderId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentNotCompleted(Guid PaymentId, Guid OrderId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodAuthorized(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodDenied(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodRefunded(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodRefundDenied(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodCancellationDenied(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record PaymentMethodCanceled(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, string Status, ulong Version) : Message, IDomainEvent;

    public record CreditCardAdded(Guid CartId, Guid MethodId, Dto.Money Amount, Dto.CreditCard CreditCard, string Status, ulong Version) : Message, IDomainEvent;

    public record DebitCardAdded(Guid CartId, Guid MethodId, Dto.Money Amount, Dto.DebitCard DebitCard, string Status, ulong Version) : Message, IDomainEvent;

    public record PayPalAdded(Guid CartId, Guid MethodId, Dto.Money Amount, Dto.PayPal PayPal, string Status, ulong Version) : Message, IDomainEvent;
}