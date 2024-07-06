﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Iter.Services.Interface;
using Iter.Services;
using Iter.Core.Models;
using Iter.Core.Options;
using RabbitMQ.Client.Exceptions;
using Polly;

class Program
{
    static IConfigurationRoot Configuration;

    static void Main(string[] args)
    {

        var retryPolicy = Policy
               .Handle<BrokerUnreachableException>()
               .WaitAndRetry(
                   retryCount: 100,
                   sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5),
                   onRetry: (exception, timeSpan, retryCount, context) =>
                   {
                       Console.WriteLine($"Pokušaj {retryCount}: Neuspešno povezivanje, pokušavam ponovo za {timeSpan.Seconds} sekundi...");
                   });

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        var rabbitMqSettings = new RabbitMqSettings();
        Configuration.GetSection(nameof(RabbitMqSettings)).Bind(rabbitMqSettings);

        var emailSettings = new EmailSettings();
        Configuration.GetSection(nameof(EmailSettings)).Bind(emailSettings);

        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSettings.HostName,
            Port = rabbitMqSettings.Port,
            UserName = rabbitMqSettings.Username,
            Password = rabbitMqSettings.Password
        };
        factory.ClientProvidedName = "Rabbit Test Consumer";
        IConnection connection = retryPolicy.Execute(() => factory.CreateConnection());
        IModel channel = connection.CreateModel();


        string exchangeName = "EmailExchange";
        string routingKey = "email_queue";
        string queueName = "EmailQueue";

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, true, false, false, null);
        channel.QueueBind(queueName, exchangeName, routingKey, null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            var emailData = JsonConvert.DeserializeObject<EmailMessage>(message);
            if (emailData != null)
            {
                Console.WriteLine($"Message received: {message}");
                IEmailService emailService = new EmailService(emailSettings);
                emailService.SendEmailAsync(emailData);

                channel.BasicAck(args.DeliveryTag, false);
            }
            else
            {
                Console.WriteLine($"Problem when sending mail: {message}");
            }
        };

        channel.BasicConsume(queueName, false, consumer);

        Console.WriteLine("Waiting for messages. Press Q to quit.");

        Thread.Sleep(Timeout.Infinite);

        channel.Close();
        connection.Close();

    }
}
