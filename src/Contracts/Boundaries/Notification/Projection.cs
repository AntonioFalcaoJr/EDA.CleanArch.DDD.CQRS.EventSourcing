using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Notification;

public static class Projection
{
    public record NotificationDetails(string Id, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Communication.Protobuf.NotificationDetails(NotificationDetails notification)
            => new() { NotificationId = notification.Id };
    }

    public record NotificationMethodDetails(string Id, Guid NotificationId, Dto.INotificationOption Option, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Communication.Protobuf.NotificationMethodDetails(NotificationMethodDetails method)
            => new()
            {
                MethodId = method.Id,
                NotificationId = method.NotificationId.ToString(),
                Option = method.Option switch
                {
                    Dto.Email email => new() { Email = email },
                    Dto.Sms sms => new() { Sms = sms },
                    Dto.PushMobile pushMobile => new() { PushMobile = pushMobile },
                    Dto.PushWeb pushWeb => new() { PushWeb = pushWeb },
                    _ => default
                }
            };
    }
}