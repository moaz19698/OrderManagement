using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Messaging
{


        public class RabbitMQConsumer
        {
            private readonly string _hostname;
            private readonly string _queueName;
            private readonly Func<string, Task> _messageProcessor;

            public RabbitMQConsumer(string hostname, string queueName, Func<string, Task> messageProcessor)
            {
                _hostname = hostname;
                _queueName = queueName;
                _messageProcessor = messageProcessor;
            }

            public void Start()
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await _messageProcessor(message);
                };

                channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            }
        }
   

}
