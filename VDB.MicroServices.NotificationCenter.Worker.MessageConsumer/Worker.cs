using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.NotificationCenter.Concern.Options;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Worker.MessageConsumer
{
    public class Worker : BackgroundService
    {
        private IConnection RabbitMQConnection;
        private IModel RabbitMQChannel;

        private readonly ILogger<Worker> Logger;
        private readonly MessageBrokerSettings MessageBrokerSettings;
        private readonly MessageBrokerSecrets MessageBrokerSecrets;
        private readonly IDatabase Cache;
        private readonly ISendNotificationManager NotificationManager;

        public Worker(IDatabase cache, ILogger<Worker> logger, IOptions<MessageBrokerSettings> messageBrokerSettings, IOptions<MessageBrokerSecrets> messageBrokerSecrets, ISendNotificationManager notificationManager)
        {
            this.Cache = cache;
            this.Logger = logger;
            this.MessageBrokerSettings = messageBrokerSettings.Value;
            this.MessageBrokerSecrets = messageBrokerSecrets.Value;
            this.NotificationManager = notificationManager;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            ConnectionFactory rabbitMQConnectionFactory = new()
            {
                HostName = this.MessageBrokerSettings.HostName,
                Port = this.MessageBrokerSettings.Port,
                UserName = this.MessageBrokerSecrets.UserName,
                Password = this.MessageBrokerSecrets.Password,
                DispatchConsumersAsync = true
            };
            this.RabbitMQConnection = rabbitMQConnectionFactory.CreateConnection();
            this.RabbitMQChannel = this.RabbitMQConnection.CreateModel();
            this.RabbitMQChannel.QueueDeclare(this.MessageBrokerSettings.NotificationQueueName, durable: true, exclusive: false, autoDelete: false);
            this.RabbitMQChannel.BasicQos(this.MessageBrokerSettings.PrefetchSize, this.MessageBrokerSettings.PrefetchCount, false);

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            this.RabbitMQConnection.Close();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            AsyncEventingBasicConsumer consumer = new(this.RabbitMQChannel);
            consumer.Received += async (bc, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                SendNotificationRequestModel request = message.DeserializeJSON<SendNotificationRequestModel>();
                try
                {
                    await this.NotificationManager.SendNotification(request);
                    this.RabbitMQChannel.BasicAck(ea.DeliveryTag, false);
                    this.Cache.HashDelete(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                }
                catch (Exception e)
                {
                    double retriedCount = this.Cache.HashIncrement(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                    this.Logger.LogError(default, e, e.Message);
                    this.RabbitMQChannel.BasicNack(ea.DeliveryTag, false, requeue: retriedCount < this.MessageBrokerSettings.RetryCount);
                }
            };

            this.RabbitMQChannel.BasicConsume(queue: this.MessageBrokerSettings.NotificationQueueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
