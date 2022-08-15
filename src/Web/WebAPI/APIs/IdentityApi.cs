using System.ComponentModel.DataAnnotations;
using Contracts.Query;
using Contracts.Services.Identity;
using MassTransit;
using MiniValidation;
using WebAPI.Abstractions;

namespace WebAPI.APIs;

public static class IdentityApi
{
    public static void MapIdentityApi(this RouteGroupBuilder group)
    {
        group.MapQuery("/login", ([AsParameters] LoginRequest request)
            => MiniValidator.TryValidate(request, out var errors)
                ? Results.Ok(request.Client.LoginAsync(new() {Email = request.Email, Password = request.Password}, cancellationToken: request.CancellationToken).ResponseAsync)
                : Results.ValidationProblem(errors));

        group.MapCommand(builder => builder.MapPost("/", (IBus bus, Command.RegisterUser command, CancellationToken ct)
            => ApplicationApi.SendCommandAsync(bus, command, ct)));

        group.WithMetadata(new TagsAttribute("IdentitiesV2"));
    }
}

internal record struct LoginRequest(IdentityService.IdentityServiceClient Client, [Required] string Email, [Required] string Password, CancellationToken CancellationToken);