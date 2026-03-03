namespace ALCI.OrderStream.Domain.Enums;

public enum OrderStatus
{
    Created,
    PaymentConfirmed,
    Processing,
    Shipped,
    Delivered
}
