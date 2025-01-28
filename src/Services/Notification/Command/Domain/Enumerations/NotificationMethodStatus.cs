using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class NotificationMethodStatus(string name, int value) : SmartEnum<NotificationMethodStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly NotificationMethodPending Pending = new();
    public static readonly NotificationMethodCancelled Cancelled = new();
    public static readonly NotificationMethodSent Sent = new();
    public static readonly NotificationMethodFailed Failed = new();

    public static explicit operator NotificationMethodStatus(int value) => FromValue(value);
    public static explicit operator NotificationMethodStatus(string name) => FromName(name);
    public static implicit operator int(NotificationMethodStatus status) => status.Value;
    public static implicit operator string(NotificationMethodStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : NotificationMethodStatus(nameof(Undefined), 0);
public class NotificationMethodPending() : NotificationMethodStatus(nameof(Pending), 1);
public class NotificationMethodCancelled() : NotificationMethodStatus(nameof(Cancelled), 2);
public class NotificationMethodSent() : NotificationMethodStatus(nameof(Sent), 3);
public class NotificationMethodFailed() : NotificationMethodStatus(nameof(Failed), 4);