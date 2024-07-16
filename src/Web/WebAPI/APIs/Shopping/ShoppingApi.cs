using Asp.Versioning.Builder;
using Contracts.Shopping.Queries;
using WebAPI.Abstractions;
using static Contracts.Shopping.Queries.ShoppingQueryService;


namespace WebAPI.APIs.Shopping;

public static class ShoppingApi
{
    private const string BaseUrl = "/api/v{version:apiVersion}/shopping/";

    public static IVersionedEndpointRouteBuilder MapShoppingApiV1(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(1);

        group.MapPost("/start-shopping/", ([AsParameters] Commands.StartShopping startShopping)
            => ApplicationApi.SendAsync(startShopping, (client, ct)
                => client.StartShoppingAsync(startShopping, cancellationToken: ct)));
        
        group.MapPost("carts/{cartId}/items", ([AsParameters] Commands.AddCartItem addCartItem)
            => ApplicationApi.SendAsync(addCartItem, (client, ct)
                => client.AddItemAsync(addCartItem, cancellationToken: ct)));
        
        group.MapPut("carts/{cartId}/check-out", ([AsParameters] Commands.CheckOut checkOut)
            => ApplicationApi.SendCommandAsync(checkOut));

        // group.MapPut("/{cartId:guid}/add-shipping-address", ([AsParameters] Commands.AddShippingAddress addShippingAddress)
        //     => ApplicationApi.SendCommandAsync(addShippingAddress));
        //
        // group.MapPut("/{cartId:guid}/add-billing-address", ([AsParameters] Commands.AddBillingAddress addBillingAddress)
        //     => ApplicationApi.SendCommandAsync(addBillingAddress));

        group.MapGet("/{cartId:guid}/details", ([AsParameters] Queries.GetShoppingCartDetails query)
            => ApplicationApi.GetAsync<ShoppingQueryServiceClient, ShoppingCartDetails>
                (query, (client, ct) => client.GetShoppingCartDetailsAsync(query, cancellationToken: ct)));

        // group.MapPost("/{cartId:guid}/items", ([AsParameters] Commands.AddItem request)
        //     => ApplicationApi.SendCommandAsync(request));

        group.MapGet("/{cartId:guid}/items/list-item", ([AsParameters] Queries.ListShoppingCartItemsListItems query)
            => ApplicationApi.ListAsync<ShoppingQueryServiceClient, ShoppingCartItemListItem>
                (query, (client, ct) => client.ListShoppingCartItemsListItemsAsync(query, cancellationToken: ct)));

        // group.MapDelete("/{cartId:guid}/items/{itemId:guid}", ([AsParameters] Commands.RemoveCartItem request)
        //     => ApplicationApi.SendCommandAsync(request));

        group.MapGet("/{cartId:guid}/items/{itemId:guid}/details", ([AsParameters] Queries.GetShoppingCartItemDetails query)
            => ApplicationApi.GetAsync<ShoppingQueryServiceClient, ShoppingCartDetails>
                (query, (client, ct) => client.GetShoppingCartItemDetailsAsync(query, cancellationToken: ct)));

        group.MapPut("/{cartId:guid}/items/{itemId:guid}/change-quantity", ([AsParameters] Commands.ChangeCartItemQuantity changeCartItemQuantity)
            => ApplicationApi.SendCommandAsync(changeCartItemQuantity));

        // group.MapPost("/{cartId:guid}/payment-methods/credit-card", ([AsParameters] Commands.AddCreditCard addCreditCard)
        //     => ApplicationApi.SendCommandAsync(addCreditCard));

        // group.MapPost("/{cartId:guid}/payment-methods/debit-card", ([AsParameters] Commands.AddDebitCard addDebitCard)
        //     => ApplicationApi.SendCommandAsync(addDebitCard));

        // group.MapPost("/{cartId:guid}/payment-methods/pay-pal", ([AsParameters] Commands.AddPayPal addPayPal)
        //     => ApplicationApi.SendCommandAsync(addPayPal));

        group.MapGet("/{cartId:guid}/payment-methods/list-items", ([AsParameters] Queries.ListPaymentMethodsListItems query)
            => ApplicationApi.ListAsync<ShoppingQueryServiceClient, PaymentMethodListItem>
                (query, (client, ct) => client.ListPaymentMethodsListItemsAsync(query, cancellationToken: ct)));

        // group.MapDelete("/{cartId:guid}/payment-methods/{methodId:guid}", ([AsParameters] Commands.RemovePaymentMethod removePaymentMethod)
        //     => ApplicationApi.SendCommandAsync(removePaymentMethod));

        group.MapGet("/{cartId:guid}/payment-methods/{methodId:guid}/details", ([AsParameters] Queries.GetPaymentMethodDetails query)
            => ApplicationApi.GetAsync<ShoppingQueryServiceClient, PaymentMethodDetails>
                (query, (client, ct) => client.GetPaymentMethodDetailsAsync(query, cancellationToken: ct)));

        group.MapGet("/customers/{customerId:guid}/cart-details", ([AsParameters] Queries.GetCustomerShoppingCartDetails query)
            => ApplicationApi.GetAsync<ShoppingQueryServiceClient, ShoppingCartDetails>
                (query, (client, ct) => client.GetCustomerShoppingCartDetailsAsync(query, cancellationToken: ct)));

        group.MapPost("/admin/rebuild-projection", ([AsParameters] Commands.RebuildProjection rebuildProjection)
            => ApplicationApi.SendCommandAsync(rebuildProjection));

        return builder;
    }

    public static IVersionedEndpointRouteBuilder MapShoppingApiV2(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet("/{cartId}/details", ([AsParameters] Queries.GetShoppingCartDetails query)
            => ApplicationApi.GetAsync<ShoppingQueryServiceClient, ShoppingCartDetails>
                (query, (client, ct) => client.GetShoppingCartDetailsAsync(query, cancellationToken: ct)));

        return builder;
    }
}