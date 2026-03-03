using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ALCI.OrderStream.Client.Pages;
using ALCI.OrderStream.Domain.Enums;
using ALCI.OrderStream.Domain.Models;
using Microsoft.JSInterop;
using Moq;

namespace ALCI.OrderStream.Client.Tests;

public class OrderTrackingComponentTests : BunitContext
{
    public OrderTrackingComponentTests()
    {        
        var configMock = new Dictionary<string, string> {
            {"ApiSettings:BaseUrl", "http://localhost:5000"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configMock!)
            .Build();
        Services.AddSingleton<IConfiguration>(configuration);
                
        JSInterop.SetupModule("./sse.js");                
        JSInterop.SetupModule("window.setupSSE", _ => true);
    }

    [Fact]
    public void OrderTracking_ShouldRender_InitialState()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var cut = Render<OrderTracking>(parameters => parameters
            .Add(p => p.OrderId, orderId)
        );

        // Assert
        Assert.Contains("Order Tracking", cut.Markup);
    }

    [Fact]
    public void OrderTracking_ShouldShowUpdate_WhenOnOrderUpdateReceivedIsCalled()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var cut = Render<OrderTracking>(parameters => parameters
            .Add(p => p.OrderId, orderId)
        );

        var update = new OrderStatusUpdate(orderId, OrderStatus.Processing, DateTime.UtcNow);

        // Act
        cut.InvokeAsync(() => cut.Instance.OnOrderUpdateReceived(update));

        // Assert
        cut.WaitForAssertion(() =>
            Assert.Contains("Warehouse Processing", cut.Markup)
        );
    }
}