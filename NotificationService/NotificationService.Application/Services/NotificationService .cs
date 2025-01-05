using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string message)
        {
            // Simulate sending notification
            await Task.Run(() =>
            {
                Console.WriteLine($"Notification sent: {message}");
            });
        }
    }
}
