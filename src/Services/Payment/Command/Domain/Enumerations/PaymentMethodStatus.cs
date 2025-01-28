using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class PaymentMethodStatus(string name, int value) : SmartEnum<PaymentMethodStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly PaymentMethodPending Pending = new();
    public static readonly PaymentMethodAuthorized Authorized = new();
    public static readonly PaymentMethodCancelled Cancelled = new();
    public static readonly PaymentMethodCancellationDenied CancellationDenied = new();
    public static readonly PaymentMethodDenied Denied = new();
    public static readonly PaymentMethodRefundDenied RefundDenied = new();
    public static readonly PaymentMethodRefunded Refunded = new();

    public static explicit operator PaymentMethodStatus(int value) => FromValue(value);
    public static explicit operator PaymentMethodStatus(string name) => FromName(name);
    public static implicit operator int(PaymentMethodStatus status) => status.Value;
    public static implicit operator string(PaymentMethodStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : PaymentMethodStatus(nameof(Undefined), 0);
public class PaymentMethodPending() : PaymentMethodStatus(nameof(Pending), 1);
public class PaymentMethodAuthorized() : PaymentMethodStatus(nameof(Authorized), 2);
public class PaymentMethodCancelled() : PaymentMethodStatus(nameof(Cancelled), 3);
public class PaymentMethodCancellationDenied() : PaymentMethodStatus(nameof(CancellationDenied), 4);
public class PaymentMethodDenied() : PaymentMethodStatus(nameof(Denied), 5);
public class PaymentMethodRefundDenied() : PaymentMethodStatus(nameof(RefundDenied), 6);
public class PaymentMethodRefunded() : PaymentMethodStatus(nameof(Refunded), 7);