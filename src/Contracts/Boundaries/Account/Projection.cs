using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Account;

public static class Projection
{
    public record AccountDetails(string Id, string FirstName, string LastName, string Email, string Status, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Account.Protobuf.AccountDetails(AccountDetails account)
            => new()
            {
                AccountId = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email
            };
    }

    public record BillingAddressListItem(string Id, string AccountId, Dto.Address Address, bool IsDeleted, ulong Version)
        : AddressListItem(Id, AccountId, Address, IsDeleted, Version);

    public record ShippingAddressListItem(string Id, string AccountId, Dto.Address Address, bool IsDeleted, ulong Version)
        : AddressListItem(Id, AccountId, Address, IsDeleted, Version);

    public abstract record AddressListItem(string Id, string AccountId, Dto.Address Address, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Account.Protobuf.AddressListItem(AddressListItem item)
        {
            return new()
            {
                AddressId = item.Id,
                AccountId = item.AccountId,
                Address = item.Address
            };
        }
    }
}