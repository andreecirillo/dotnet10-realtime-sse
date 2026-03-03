namespace ALCI.OrderStream.Domain.Models;

using ALCI.OrderStream.Domain.Enums;

public sealed record OrderStatusUpdate(
        Guid OrderId,
        OrderStatus? Status,
        DateTime Timestamp
    );

