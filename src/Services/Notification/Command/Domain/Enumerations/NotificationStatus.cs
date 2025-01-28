using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class NotificationStatus(string name, int value) : SmartEnum<NotificationStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly NotificationActive Active = new();
    public static readonly NotificationDelivered Delivered = new();
    public static readonly NotificationFailed Failed = new();
    public static readonly NotificationCanceled Canceled = new();

    public static explicit operator NotificationStatus(int value) => FromValue(value);
    public static explicit operator NotificationStatus(string name) => FromName(name);
    public static implicit operator int(NotificationStatus status) => status.Value;
    public static implicit operator string(NotificationStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : NotificationStatus(nameof(Undefined), 0);
public class NotificationActive() : NotificationStatus(nameof(Active), 1);
public class NotificationDelivered() : NotificationStatus(nameof(Delivered), 2);
public class NotificationFailed() : NotificationStatus(nameof(Failed), 3);
public class NotificationCanceled() : NotificationStatus(nameof(Canceled), 4);