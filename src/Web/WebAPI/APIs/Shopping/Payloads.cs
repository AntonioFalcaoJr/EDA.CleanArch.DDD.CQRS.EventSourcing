using Contracts.DataTransferObjects;

namespace WebAPI.APIs.Shopping;

public static class Payloads
{
    public record StartShopping(string CustomerId);
    public record AddCartItem(string ProductId, int Quantity);
    public record AddCreditCard(Dto.Money Amount, Dto.CreditCard CreditCard);
    public record AddDebitCard(Dto.Money Amount, Dto.DebitCard DebitCard);
    public record AddPaypal(Dto.Money Amount, Dto.PayPal PayPal);
}