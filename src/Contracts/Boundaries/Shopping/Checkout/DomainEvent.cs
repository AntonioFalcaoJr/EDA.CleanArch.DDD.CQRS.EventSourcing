using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Shopping.Checkout;

public static class DomainEvent
{
    public record CheckoutStarted(string CheckoutId, string CartId, ulong Version) : Message, IDomainEvent;

    public record BillingAddressAdded(string CheckoutId, string CartId, string City, string Complement, string Country, string Number, string State, string Street, string ZipCode, ulong Version) : Message, IDomainEvent;

    public record DebitCardAdded(string CheckoutId, string CartId, string ExpirationDate, string Number, string HolderName, string Cvv, ulong Version) : Message, IDomainEvent;

    public record CreditCardAdded(string CheckoutId, string CartId, string ExpirationDate, string Number, string HolderName, string Cvv, ulong Version) : Message, IDomainEvent;

    public record PayPalAdded(string CheckoutId, string CartId, string Email, string Password, ulong Version) : Message, IDomainEvent;

    public record ShippingAddressAdded(string CheckoutId, string CartId, string City, string Complement, string Country, string Number, string State, string Street, string ZipCode, ulong Version) : Message, IDomainEvent;
}