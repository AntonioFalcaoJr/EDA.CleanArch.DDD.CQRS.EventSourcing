using Contracts.Abstractions;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Boundaries.Identity;

public static class Projection
{
    public record UserDetails(string Id, string FirstName, string LastName, string Email, string Password, bool IsDeleted, ulong Version, [property: BsonIgnore] string? Token = default) : IProjection
    {
        public static implicit operator Services.Identity.Protobuf.UserDetails(UserDetails userDetails)
            => new()
            {
                UserId = userDetails.Id,
                Email = userDetails.Email,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Token = userDetails.Token
            };
    }
}