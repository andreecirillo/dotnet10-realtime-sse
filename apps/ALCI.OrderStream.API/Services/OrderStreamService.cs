using System.Collections.Concurrent;
using System.Threading.Channels;
using ALCI.OrderStream.Domain.Models;

namespace ALCI.OrderStream.API.Services
{
    public sealed class OrderStreamService
    {
        private readonly ILogger<OrderStreamService> _logger;
        private readonly ConcurrentDictionary<Guid, Channel<OrderStatusUpdate>> _streams = new();        

        public OrderStreamService(ILogger<OrderStreamService> logger)
        {
            _logger = logger;
        }

        public ChannelReader<OrderStatusUpdate> Subscribe(Guid orderId)
        {
            var channel = Channel.CreateUnbounded<OrderStatusUpdate>();
            _streams[orderId] = channel;

            const string subscriptionLogTemplate = "📡 [STREAM] New subscription established: {OrderId}";
            _logger.LogWarning(subscriptionLogTemplate, orderId.ToShortId());

            return channel.Reader;
        }

        public async Task PublishAsync(OrderStatusUpdate update)
        {
            if (_streams.TryGetValue(update.OrderId, out var channel))
            {
                await channel.Writer.WriteAsync(update);

                const string updateLogTemplate = "⚡ [EVENT] Update sent: {Status} for Order {OrderId}";
                _logger.LogWarning(updateLogTemplate,
                    update.Status,
                    update.OrderId.ToShortId());
            }
        }

        public async Task PublishFinalizeAsync(Guid orderId)
        {
            if (_streams.TryGetValue(orderId, out var channel))
            {                
                await channel.Writer.WriteAsync(new OrderStatusUpdate(orderId, null, DateTime.UtcNow));
            }
        }

        public void Unsubscribe(Guid orderId)
        {
            if (_streams.TryRemove(orderId, out var channel))
            {
                channel.Writer.TryComplete();

                const string closedTemplate = "🛑 [CLOSED] Stream removed: {OrderId}";
                _logger.LogWarning(closedTemplate, orderId.ToShortId());
            }
        }
    }

    internal static class OrderStreamServiceExtensions
    {
        public static string ToShortId(this Guid id) => id.ToString()[..8];
    }
}