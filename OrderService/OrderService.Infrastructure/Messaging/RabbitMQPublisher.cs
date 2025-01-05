using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
namespace OrderService.Infrastructure.Messaging
{


    public class RabbitMQPublisher
    {
        private readonly string _hostname;
        private readonly string _queueName;

        public RabbitMQPublisher(string hostname, string queueName)
        {
            _hostname = hostname;
            _queueName = queueName;
        }

        public async Task PublishAsync<T>(T message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            // Await the connection task
            using var connection = await Task.Run(() => factory.CreateConnection());
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }

    }
}
