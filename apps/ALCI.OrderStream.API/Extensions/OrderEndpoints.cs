using ALCI.OrderStream.API.Services;
using ALCI.OrderStream.Domain.Enums;
using ALCI.OrderStream.Domain.Models;

namespace ALCI.OrderStream.API.Extensions;

public static class OrderEndpointsExtensions
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders");
                
        group.MapGet("/{orderId:guid}/stream", OrderUpdateStream)
            .WithName("OrderUpdateStream")            
            .ExcludeFromDescription();

        group.MapPost("/{orderId:guid}/simulate", SimulateOrderUpdate)
            .WithName("SimulateOrderUpdate")
            .WithSummary("Simulates order status updates for the active stream")
            .WithDescription("Default Test ID: `550e8400-e29b-41d4-a716-446655440000`");
    }

    private static IResult OrderUpdateStream(Guid orderId, OrderStreamService service, CancellationToken ct)
    {        
        var updateSource = service.Subscribe(orderId).ReadAllAsync(ct);
                
        return Results.ServerSentEvents(updateSource, eventType: "order-update");
    }

    private static IResult SimulateOrderUpdate(Guid orderId, OrderStreamService streamService)
    {        
        _ = Task.Run(async () =>
        {
            var statuses = Enum.GetValues<OrderStatus>();
            foreach (var status in statuses)
            {
                await Task.Delay(2000);
                await streamService.PublishAsync(new OrderStatusUpdate(orderId, status, DateTime.UtcNow));
            }

            await streamService.PublishFinalizeAsync(orderId);

            streamService.Unsubscribe(orderId);
        });

        return Results.Accepted();
    }
}