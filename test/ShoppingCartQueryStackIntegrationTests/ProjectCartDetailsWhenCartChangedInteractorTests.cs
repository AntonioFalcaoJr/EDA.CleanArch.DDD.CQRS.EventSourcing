using Contracts.Abstractions.Protobuf;
using Contracts.Boundaries.Shopping.ShoppingCart;
using Contracts.Shopping.Queries;
using FluentAssertions;
using GrpcService;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Xunit;

namespace ShoppingCartQueryStackIntegrationTests;

public class ProjectCartDetailsWhenCartChangedInteractorTests(ShoppingCartServiceClientFixture factory) : IClassFixture<ShoppingCartServiceClientFixture>
{
    private readonly IPublishEndpoint _publishEndpoint = factory.Services.GetRequiredService<IPublishEndpoint>();
    private readonly ShoppingCartGrpcService _client = factory.Services.GetRequiredService<ShoppingCartGrpcService>();

    [Fact]
    public async Task CartDetailsShouldBeProjectedWhenCartCreated()
    {
        // Given
        DomainEvent.ShoppingStarted @event = new(
            CartId: Guid.NewGuid().ToString(),
            CustomerId: Guid.NewGuid().ToString(),
            Status: "Open",
            Version: 1);

        // When
        await _publishEndpoint.Publish(@event, CancellationToken.None);
        
        var response = await Policy
            .HandleResult<GetResponse>(response => response.OneOfCase is GetResponse.OneOfOneofCase.NotFound)
            .RetryAsync()
            .ExecuteAsync(() => _client.GetShoppingCartDetails(new() { CartId = @event.CartId }, default!));

        // Then
        response.NotFound.Should().BeNull();
        response.Projection.TryUnpack<ShoppingCartDetails>(out var projection).Should().BeTrue();
        
        projection.CartId.Should().Be(@event.CartId);
        projection.CustomerId.Should().Be(@event.CustomerId);
        projection.Status.Should().Be(@event.Status);
    }
}