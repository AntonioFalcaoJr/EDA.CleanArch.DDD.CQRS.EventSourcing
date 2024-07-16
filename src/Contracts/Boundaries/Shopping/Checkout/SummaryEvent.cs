using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Shopping.Checkout;

public static class SummaryEvent
{
    public record CartProjectionRebuilt(Dto.ShoppingCart Cart, ulong Version) : Message, ISummaryEvent;

    public record CartSubmitted(Dto.ShoppingCart Cart, ulong Version) : Message, ISummaryEvent;
}