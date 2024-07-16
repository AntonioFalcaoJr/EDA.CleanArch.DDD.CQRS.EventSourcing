using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Notification;

public static class DomainEvent
{
    public record NotificationRequested(Guid NotificationId, IEnumerable<Dto.NotificationMethod> Methods, ulong Version) : Message, IDomainEvent;

    public record NotificationMethodFailed(Guid NotificationId, Guid MethodId, ulong Version) : Message, IDomainEvent;

    public record NotificationMethodSent(Guid NotificationId, Guid MethodId, ulong Version) : Message, IDomainEvent;

    public record NotificationMethodCancelled(Guid NotificationId, Guid MethodId, ulong Version) : Message, IDomainEvent;

    public record NotificationMethodReset(Guid NotificationId, Guid MethodId, ulong Version) : Message, IDomainEvent;
}