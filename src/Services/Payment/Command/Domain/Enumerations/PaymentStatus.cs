using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class PaymentStatus(string name, int value) : SmartEnum<PaymentStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly PaymentReady Ready = new();
    public static readonly PaymentCompleted Completed = new();
    public static readonly PaymentNotCompleted NotCompleted = new();
    public static readonly PaymentCancelled Cancelled = new();
    public static readonly PaymentRefunded Refunded = new();

    public static explicit operator PaymentStatus(int value) => FromValue(value);
    public static explicit operator PaymentStatus(string name) => FromName(name);
    public static implicit operator int(PaymentStatus status) => status.Value;
    public static implicit operator string(PaymentStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : PaymentStatus(nameof(Undefined), 0);
public class PaymentReady() : PaymentStatus(nameof(Ready), 1);
public class PaymentCompleted() : PaymentStatus(nameof(Completed), 2);
public class PaymentNotCompleted() : PaymentStatus(nameof(NotCompleted), 3);
public class PaymentCancelled() : PaymentStatus(nameof(Cancelled), 4);
public class PaymentRefunded() : PaymentStatus(nameof(Refunded), 5);