using Iter.Core.Options;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
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
            this.logger.LogInformation("Prije factorija");
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.HostName,
                Port = rabbitMqSettings.Port,
                UserName = rabbitMqSettings.Username,
                Password = rabbitMqSettings?.Password,
            };
            factory.ClientProvidedName = "Rabbit";

            this.logger.LogInformation("Prije connection");
            IConnection connection = factory.CreateConnection();
            this.logger.LogInformation("posle connection");

            IModel channel = connection.CreateModel();
            this.logger.LogInformation("posle CreateModel");

            string exchangeName = "EmailExchange";
            string routingKey = "email_queue";
            string queueName = "EmailQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            string emailModelJson = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(emailModelJson);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
        }
    }
}