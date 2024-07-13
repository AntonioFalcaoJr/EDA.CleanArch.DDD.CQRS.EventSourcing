using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Domain.Abstractions.Identities;
using Domain.Aggregates.Checkouts;
using Domain.Aggregates.Products;
using Domain.Aggregates.ShoppingCarts;
using Infrastructure.EventStore.Contexts.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Version = Domain.ValueObjects.Version;

namespace Infrastructure.EventStore.Contexts.Configurations;

public class ShoppingCartStoreEventConfiguration : StoreEventConfiguration<ShoppingCart, CartId>;
public class CheckoutStoreEventConfiguration : StoreEventConfiguration<Checkout, CheckoutId>;
public class ProductStoreEventConfiguration : StoreEventConfiguration<Product, ProductId>;

public abstract class StoreEventConfiguration<TAggregate, TId> : IEntityTypeConfiguration<StoreEvent<TAggregate, TId>>
    where TAggregate : AggregateRoot<TId>
    where TId : GuidIdentifier, new()
{
    public void Configure(EntityTypeBuilder<StoreEvent<TAggregate, TId>> builder)
    {
        builder.ToTable($"{typeof(TAggregate).Name}StoreEvents");

        builder.HasKey(@event => new { @event.Version, @event.AggregateId });

        builder
            .Property(@event => @event.AggregateId)
            .HasConversion<Guid>(id => id, guid => GuidIdentifier.New<TId>(guid))
            .IsRequired();

        builder
            .Property(@event => @event.Event)
            .HasConversion<EventConverter>()
            .IsRequired();

        builder
            .Property(@event => @event.EventType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

        builder
            .Property(@event => @event.Timestamp)
            .IsRequired();

        builder
            .Property(@event => @event.Version)
            .HasConversion<ushort>(version => version, number => Version.Number(number))
            .IsRequired();
    }
}