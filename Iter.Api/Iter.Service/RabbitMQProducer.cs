using Iter.Core.Options;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Iter.Services
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly ILogger<RabbitMQProducer> logger;
        private readonly RabbitMqSettings rabbitMqSettings;

        public RabbitMQProducer(ILogger<RabbitMQProducer> logger, RabbitMqSettings rabbitMqSettings)
        {
            this.logger = logger;
            this.rabbitMqSettings = rabbitMqSettings;
        }

        public void SendMessage<T>(T message)
        {
            try
            {
                logger.LogInformation("Starting to send message to RabbitMQ.");

                var factory = new ConnectionFactory
                {
                    HostName = rabbitMqSettings.HostName,
                    Port = rabbitMqSettings.Port,
                    UserName = rabbitMqSettings.Username,
                    Password = rabbitMqSettings?.Password,
                    ClientProvidedName = "Rabbit"
                };

                logger.LogInformation("Connecting to RabbitMQ server at {Host}:{Port}", factory.HostName, factory.Port);
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                string exchangeName = "EmailExchange";
                string routingKey = "email_queue";
                string queueName = "EmailQueue";

                logger.LogInformation("Declaring exchange: {ExchangeName} and queue: {QueueName}", exchangeName, queueName);
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);

                string messageJson = JsonConvert.SerializeObject(message);
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(messageJson);

                logger.LogInformation("Publishing message to exchange: {ExchangeName} with routing key: {RoutingKey}", exchangeName, routingKey);
                channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

                logger.LogInformation("Message sent successfully to RabbitMQ.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while sending message to RabbitMQ.");
                throw;
            }
        }
    }
}
