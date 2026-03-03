using ALCI.OrderStream.API.Services;
using ALCI.OrderStream.Domain.Enums;
using ALCI.OrderStream.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace ALCI.OrderStream.API.Tests;

public class OrderStreamServiceUnitTests
{
    private readonly OrderStreamService _service;
    private readonly Mock<ILogger<OrderStreamService>> _loggerMock;

    public OrderStreamServiceUnitTests()
    {
        _loggerMock = new Mock<ILogger<OrderStreamService>>();
        _service = new OrderStreamService(_loggerMock.Object);
    }

    [Fact]
    public async Task Subscribe_ShouldReturnReader_AndReceivePublishedUpdates()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var update = new OrderStatusUpdate(orderId, OrderStatus.Processing, DateTime.UtcNow);
        var reader = _service.Subscribe(orderId);

        // Act
        await _service.PublishAsync(update);
        var received = await reader.ReadAsync();

        // Assert
        Assert.NotNull(received);
        Assert.Equal(orderId, received.OrderId);
        Assert.Equal(OrderStatus.Processing, received.Status);
    }

    [Fact]
    public void Unsubscribe_ShouldCompleteChannel()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reader = _service.Subscribe(orderId);

        // Act
        _service.Unsubscribe(orderId);

        // Assert
        Assert.True(reader.Completion.IsCompleted);
    }

    [Fact]
    public async Task PublishFinalize_ShouldSendUpdateWithNullStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reader = _service.Subscribe(orderId);

        // Act
        await _service.PublishFinalizeAsync(orderId);
        var received = await reader.ReadAsync();

        // Assert
        Assert.Null(received.Status);
    }
}