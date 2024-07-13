using Domain.Abstractions;

namespace Domain;

public static class Exceptions
{
    public class CartNotOpen() : DomainException<CartNotOpen>("Cart is not open.");
    public class CartIsEmpty() : DomainException<CartIsEmpty>("Cart is empty.");
    public class CartNotAvailable() : DomainException<CartNotAvailable>("Cart is already in use.");
    public class CartItemNotFound() : DomainException<CartItemNotFound>("Cart item not found.");
    public class CartItemQuantityInvalid() : DomainException<CartItemQuantityInvalid>("Cart item quantity is invalid.");
    public class WrongCartException() : DomainException<WrongCartException>("The cart is not the one expected.");
    public class CartItemPriceInvalid() : DomainException<CartItemPriceInvalid>("Cart item price is invalid.");
    public class CartItemPriceCurrencyInvalid() : DomainException<CartItemPriceCurrencyInvalid>("Cart item price currency is invalid.");
    public class InvalidCardNumber() : DomainException<InvalidCardNumber>("Invalid card number.");
    public class InvalidMonth() : DomainException<InvalidMonth>("Invalid month.");
    public class InvalidQuantity() : DomainException<InvalidQuantity>("Invalid quantity.");
    public class InvalidPaymentMethodException() : DomainException<InvalidPaymentMethodException>("Invalid payment method.");
    public class InvalidSecurityCode() : DomainException<InvalidSecurityCode>("Invalid security code.");
    public class InvalidYear() : DomainException<InvalidYear>("Invalid year.");
    public class InvalidCardholderName() : DomainException<InvalidCardholderName>("Invalid cardholder name.");
    public class InvalidIdentifier() : DomainException<InvalidIdentifier>("Invalid identifier.");
    public class AggregateNotFound() : DomainException<AggregateNotFound>("Aggregate not found.");
    public class ProductInventoryNotEnough() : DomainException<ProductInventoryNotEnough>("Product inventory is not enough");
    public class AggregateIsDeleted() : DomainException<AggregateIsDeleted>("Aggregate is deleted.");
    public class VersionMustBePositive() : DomainException<VersionMustBePositive>("Version must be a positive number.");
    public class VersionFormatException() : DomainException<VersionFormatException>("Version must be a positive number.");
}