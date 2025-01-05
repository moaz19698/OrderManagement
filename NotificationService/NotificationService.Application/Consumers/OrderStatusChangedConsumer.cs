using NotificationService.Application.Interfaces;
using NotificationService.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.Application.Consumers
{
    public class OrderStatusChangedConsumer
    {
        private readonly INotificationService _notificationService;

        public OrderStatusChangedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task ConsumeAsync(string message)
        {
            var orderStatusEvent = JsonSerializer.Deserialize<OrderStatusChangedEvent>(message);
            if (orderStatusEvent != null)
            {
                var notificationMessage = $"Order {orderStatusEvent.OrderId} status changed from {orderStatusEvent.OldStatus} to {orderStatusEvent.NewStatus} at {orderStatusEvent.ChangedAt}.";
                await _notificationService.SendNotificationAsync(notificationMessage);
            }
        }
    }
}
