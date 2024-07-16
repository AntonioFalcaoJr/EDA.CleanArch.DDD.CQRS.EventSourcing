using AutoFixture;
using Contracts.Abstractions.Messages;
using Contracts.Boundaries.Shopping.ShoppingCart;
using Domain;
using Domain.Aggregates.Products;
using Domain.Aggregates.ShoppingCarts;
using Domain.Entities.CartItems;
using Domain.Enumerations;
using Domain.Extensions;
using Domain.ValueObjects;
using FluentAssertions;
using ShoppingCartUnitTests.Abstractions;
using Xunit;
using Size = Domain.ValueObjects.Size;
using Version = Domain.ValueObjects.Version;

namespace ShoppingCartUnitTests;

public class ShoppingCartUnitTests : AggregateTests<ShoppingCart, CartId>
{
    private static readonly Fixture Fixture = new()
    {
        Customizations =
        {
            new ElementsBuilder<Currency>(
                Currency.All.Select(currency => (Currency)currency.Key))
        }
    };

    private readonly CartId _cartId = CartId.New;
    private readonly CustomerId _customerId = CustomerId.New;
    private readonly CartItemId _itemId = CartItemId.New;

    private readonly Dictionary<Currency, Price> _prices =
        Currency.All.ToDictionary(
            currency => currency.Value,
            currency => new Price(Fixture.Create<Amount>(), currency.Value));

    private readonly ProductId _productId = ProductId.New;
    private readonly ProductName _productName = Fixture.Create<ProductName>();
    private readonly PictureUri _pictureUri = Fixture.Create<PictureUri>();

    private readonly Quantity _quantity = Fixture.Create<Quantity>();

    private readonly Sku _sku = new(
        Fixture
            .Build<Brand>()
            .FromFactory(() => new(Fixture.Create<string>()[..30], Fixture.Create<string>()[..5]))
            .Create(),
        Fixture.Create<Category>(),
        // Fixture.Create<Size>(),
        // Fixture.Create<Color>(),
        Fixture.Create<Size>());

    [Fact]
    public void StartShopping_ShouldRaise_ShoppingStarted()
        => Given()
            .When(ShoppingCart.StartShopping(_customerId))
            .Then<DomainEvent.ShoppingStarted>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CustomerId.Should().NotBe(CustomerId.Undefined),
                @event => @event.CustomerId.Should().Be(_customerId),
                @event => @event.Status.Should().Be(CartStatus.Open),
                @event => @event.Version.Should().Be(Version.Initial));

    [Fact]
    public void AddCartItem_ShouldRaise_CartItemAdded()
    {
        CartItem item = new(_itemId, _productId, _productName, _pictureUri, _sku, _prices, _quantity);
        var expectedPrices = _prices.AsString();
        var expectedTotals = Aggregate.Totals.Project(item);

        Given(new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial))
            .When(cart => cart.AddItem(item))
            .Then<DomainEvent.CartItemAdded>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CartId.Should().Be(_cartId),
                @event => @event.ItemId.Should().NotBe(CartItemId.Undefined),
                @event => @event.ItemId.Should().Be(_itemId),
                @event => @event.ProductId.Should().NotBe(ProductId.Undefined),
                @event => @event.ProductId.Should().Be(_productId),
                @event => @event.ProductName.Should().Be(_productName),
                @event => @event.PictureUri.Should().Be(_pictureUri),
                @event => @event.Sku.Should().Be(_sku),
                @event => @event.Quantity.Should().Be(_quantity),
                @event => @event.Prices.Should().BeEquivalentTo(expectedPrices),
                @event => @event.Totals.Should().BeEquivalentTo(expectedTotals),
                @event => @event.Version.Should().Be(Version.Number(2)));
    }

    [Fact]
    public void AddExistingCartItem_ShouldRaise_CartItemIncreased()
    {
        var initialQuantity = Fixture.Create<Quantity>();
        var initialTotals = Aggregate.Totals.Project(_prices, _quantity);
        CartItem item = new(_itemId, _productId, _productName, _pictureUri, _sku, _prices, initialQuantity);
        
        var expectedQuantity = item.Quantity + _quantity;
        var expectedTotals = Aggregate.Totals.Project(item.Prices, expectedQuantity);
        var expectedPrices = item.Prices.AsString();

        IDomainEvent[] stream =
        [
            new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial),
            
            new DomainEvent.CartItemAdded(_cartId, _itemId, _productId, _productName, _pictureUri,
                _sku, _quantity, expectedPrices, initialTotals, Version.Number(2))
        ];

        Given(stream)
            .When(cart => cart.AddItem(item))
            .Then<DomainEvent.CartItemIncreased>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CartId.Should().Be(_cartId),
                @event => @event.ItemId.Should().NotBe(CartItemId.Undefined),
                @event => @event.ItemId.Should().Be(_itemId),
                @event => @event.NewQuantity.Should().Be(expectedQuantity),
                @event => @event.Prices.Should().BeEquivalentTo(expectedPrices),
                @event => @event.Totals.Should().BeEquivalentTo(expectedTotals),
                @event => @event.Version.Should().Be(Version.Number(3)));
    }

    [Fact]
    public void AddCartItemInNotOpenCart_ShouldThrow_CartNotOpenException()
    {
        CartItem item = new(_itemId, _productId, _productName, _pictureUri, _sku, _prices, _quantity);

        Given()
            .When(cart => cart.AddItem(item))
            .ThenThrows<Exceptions.CartNotOpen>();
    }

    [Fact]
    public void ChangeCartItemQuantityForUp_ShouldRaise_CartItemIncreased()
    {
        var initialTotals = Aggregate.Totals.Project(_prices, _quantity);
        var increasedQuantity = _quantity + Fixture.Create<Quantity>();
        var expectedPrices = _prices.AsString();
        var expectedTotals = Aggregate.Totals.Project(_prices, increasedQuantity);

        IDomainEvent[] stream =
        [
            new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial),
            
            new DomainEvent.CartItemAdded(_cartId, _itemId, _productId, _productName, 
                _pictureUri, _sku, _quantity, expectedPrices, initialTotals, Version.Number(2))
        ];

        Given(stream)
            .When(cart => cart.ChangeItemQuantity(_productId, increasedQuantity))
            .Then<DomainEvent.CartItemIncreased>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CartId.Should().Be(_cartId),
                @event => @event.ItemId.Should().NotBe(CartItemId.Undefined),
                @event => @event.ItemId.Should().Be(_itemId),
                @event => @event.ProductId.Should().NotBe(ProductId.Undefined),
                @event => @event.ProductId.Should().Be(_productId),
                @event => @event.NewQuantity.Should().Be(increasedQuantity),
                @event => @event.Prices.Should().BeEquivalentTo(expectedPrices),
                @event => @event.Totals.Should().BeEquivalentTo(expectedTotals),
                @event => @event.Version.Should().Be(Version.Number(3)));
    }

    [Fact]
    public void ChangeCartItemQuantityForDown_ShouldRaise_CartItemDecreased()
    {
        var itemAddedTotals = Aggregate.Totals.Project(_prices, _quantity);
        var expectedQuantity = _quantity - Quantity.Number(1);
        var expectedPrices = _prices.AsString();
        var expectedTotals = Aggregate.Totals.Project(_prices, expectedQuantity);

        IDomainEvent[] stream =
        [
            new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial),
            new DomainEvent.CartItemAdded(_cartId, _itemId, _productId, _productName, _pictureUri, _sku, _quantity,
                expectedPrices, itemAddedTotals, Version.Number(2))
        ];

        Given(stream)
            .When(cart => cart.ChangeItemQuantity(_productId, expectedQuantity))
            .Then<DomainEvent.CartItemDecreased>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CartId.Should().Be(_cartId),
                @event => @event.ItemId.Should().NotBe(CartItemId.Undefined),
                @event => @event.ItemId.Should().Be(_itemId),
                @event => @event.ProductId.Should().NotBe(ProductId.Undefined),
                @event => @event.ProductId.Should().Be(_productId),
                @event => @event.NewQuantity.Should().Be(expectedQuantity),
                @event => @event.Prices.Should().BeEquivalentTo(expectedPrices),
                @event => @event.Totals.Should().BeEquivalentTo(expectedTotals),
                @event => @event.Version.Should().Be(Version.Number(3)));
    }

    [Fact]
    public void ChangeCartItemQuantityForZero_ShouldRaise_CartItemRemoved()
    {
        var itemAddedTotals = Aggregate.Totals.Project(_prices, _quantity);
        var expectedPrices = _prices.AsString();
        var expectedTotals = Aggregate.Totals.Project(_prices, Quantity.Zero);

        IDomainEvent[] stream =
        [
            new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial),
            new DomainEvent.CartItemAdded(_cartId, _itemId, _productId, _productName, _pictureUri, _sku, _quantity,
                expectedPrices, itemAddedTotals, Version.Number(2))
        ];

        Given(stream)
            .When(cart => cart.ChangeItemQuantity(_productId, Quantity.Zero))
            .Then<DomainEvent.CartItemRemoved>(
                @event => @event.CartId.Should().NotBe(CartId.Undefined),
                @event => @event.CartId.Should().Be(_cartId),
                @event => @event.ItemId.Should().NotBe(CartItemId.Undefined),
                @event => @event.ItemId.Should().Be(_itemId),
                @event => @event.ProductId.Should().NotBe(ProductId.Undefined),
                @event => @event.ProductId.Should().Be(_productId),
                @event => @event.Prices.Should().BeEquivalentTo(expectedPrices),
                @event => @event.Totals.Should().BeEquivalentTo(expectedTotals),
                @event => @event.Version.Should().Be(Version.Number(3)));
    }

    [Fact]
    public void ChangeCartItemQuantityForSame_ShouldDoNothing()

    {
        var itemAddedTotals = Aggregate.Totals.Project(_prices, _quantity);
        var itemAddedPrices = _prices.AsString();

        IDomainEvent[] stream =
        [
            new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial),
            new DomainEvent.CartItemAdded(_cartId, _itemId, _productId, _productName, _pictureUri, _sku, _quantity,
                itemAddedPrices, itemAddedTotals, Version.Number(2))
        ];

        Given(stream)
            .When(cart => cart.ChangeItemQuantity(_productId, _quantity))
            .ThenNothing();
    }

    [Fact]
    public void ChangeItemQuantity_WhenNotExist_ShouldThrow_CartNotOpenException()
        => Given(new DomainEvent.ShoppingStarted(_cartId, _customerId, CartStatus.Open, Version.Initial))
            .When(cart => cart.ChangeItemQuantity(_productId, _quantity))
            .ThenThrows<Exceptions.CartItemNotFound>();

    [Fact]
    public void ChangeItemQuantity_WhenCartIsNotOpen_ShouldThrow_CartNotOpenException()
        => Given()
            .When(cart => cart.ChangeItemQuantity(_productId, _quantity))
            .ThenThrows<Exceptions.CartNotOpen>();


    // [Fact]
    // public void RemoveCartItemShouldRaiseCartItemRemoved()
    // {
    //     var cartItemAddedNewCartTotal = _unitPrice * _quantity;
    //
    //     Given<ShoppingCart>(
    //             new DomainEvent.ShoppingStarted(_cartId, _customerId, Money.Zero(Currency.USD), CartStatus.Open, 1),
    //             new DomainEvent.CartItemAdded(_cartId, _itemId, _inventoryId, _product, _quantity, _unitPrice, cartItemAddedNewCartTotal, 2))
    //         .When<Command.RemoveCartItem>(new(_cartId, _itemId))
    //         .Then<DomainEvent.CartItemRemoved>(
    //             @event => @event.CartId.Should().Be(_cartId),
    //             @event => @event.ItemId.Should().Be(_itemId),
    //             @event => @event.Version.Should().Be(3));
    // }
    //
    // [Fact]
    // public void AddCartItemWithSameProductShouldRaiseCartItemIncreased()
    // {
    //     var cartItemAddedNewCartTotal = _unitPrice * _quantity;
    //     var addCartItemQuantity = (ushort)(_quantity + 1);
    //
    //     var expectedNewQuantity = (ushort)(_quantity + addCartItemQuantity);
    //     Dto.Money expectedNewCartTotal = cartItemAddedNewCartTotal + _unitPrice * addCartItemQuantity;
    //
    //     Given<ShoppingCart>(
    //             new DomainEvent.ShoppingStarted(_cartId, _customerId, Money.Zero(Currency.USD), CartStatus.Open, 1),
    //             new DomainEvent.CartItemAdded(_cartId, _itemId, _inventoryId, _product, _quantity, _unitPrice, cartItemAddedNewCartTotal, 2))
    //         .When<Command.AddCartItem>(new(_cartId, _catalogId, _inventoryId, _product, addCartItemQuantity, _unitPrice))
    //         .Then<DomainEvent.CartItemIncreased>(
    //             @event => @event.CartId.Should().Be(_cartId),
    //             @event => @event.ItemId.Should().Be(_itemId),
    //             @event => @event.NewQuantity.Should().Be(expectedNewQuantity),
    //             @event => @event.NewCartTotal.Should().Be(expectedNewCartTotal),
    //             @event => @event.Version.Should().Be(3));
    // }
    //
    // [Fact]
    // public void AddCartItemWithDifferentProductShouldRaiseCartItemAdded()
    // {
    //     var product = _product with { Name = _fixture.Create<string>(), Brand = _fixture.Create<string>() };
    //     var cartItemAddedNewCartTotal = _unitPrice * _quantity;
    //
    //     Dto.Money expectedUnitPrice = _unitPrice;
    //     Dto.Money expectedNewCartTotal = cartItemAddedNewCartTotal + _unitPrice * _quantity;
    //
    //     Given<ShoppingCart>(
    //             new DomainEvent.ShoppingStarted(_cartId, _customerId, Money.Zero(Currency.USD), CartStatus.Open, 1),
    //             new DomainEvent.CartItemAdded(_cartId, _itemId, _inventoryId, _product, _quantity, _unitPrice, cartItemAddedNewCartTotal, 2))
    //         .When<Command.AddCartItem>(new(_cartId, _catalogId, _inventoryId, product, _quantity, _unitPrice))
    //         .Then<DomainEvent.CartItemAdded>(
    //             @event => @event.CartId.Should().Be(_cartId),
    //             @event => @event.ItemId.Should().NotBe(_itemId),
    //             @event => @event.Quantity.Should().Be(_quantity),
    //             @event => @event.UnitPrice.Should().Be(expectedUnitPrice),
    //             @event => @event.Product.Should().NotBeEquivalentTo(_product),
    //             @event => @event.NewCartTotal.Should().Be(expectedNewCartTotal),
    //             @event => @event.Version.Should().Be(3));
    // }
    //
    // [Fact]
    // public void AddBillingAddressShouldRaiseBillingAddressAdded()
    // {
    //     var address = _fixture.Create<Address>();
    //
    //     Given<ShoppingCart>(new DomainEvent.ShoppingStarted(_cartId, _customerId, Money.Zero(Currency.USD), CartStatus.Open, 1))
    //         .When<Command.AddBillingAddress>(new(_cartId, address))
    //         .Then<DomainEvent.BillingAddressAdded>(
    //             @event => @event.CartId.Should().Be(_cartId),
    //             @event => @event.Address.Should().BeEquivalentTo(address),
    //             @event => @event.Version.Should().Be(2));
    // }
    //
    // [Fact]
    // public void AddShippingAddressShouldRaiseShippingAddressAdded()
    // {
    //     var address = _fixture.Create<Address>();
    //
    //     Given<ShoppingCart>(new DomainEvent.ShoppingStarted(_cartId, _customerId, Money.Zero(Currency.USD), CartStatus.Open, 1))
    //         .When<Command.AddShippingAddress>(new(_cartId, address))
    //         .Then<DomainEvent.ShippingAddressAdded>(
    //             @event => @event.CartId.Should().Be(_cartId),
    //             @event => @event.Address.Should().BeEquivalentTo(address),
    //             @event => @event.Version.Should().Be(2));
    // }
}