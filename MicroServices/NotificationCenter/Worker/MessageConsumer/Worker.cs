using Microsoft.Extensions.DependencyInjection;
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
using VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Worker.MessageConsumer
{
    public class Worker : BackgroundService
    {
        private IConnection RabbitMQConnection;
        private IModel NotificationRabbitMQChannel;
        private IModel TicketCreationRabbitMQChannel;

        private readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly ILogger<Worker> Logger;
        private readonly MessageBrokerSettings MessageBrokerSettings;
        private readonly MessageBrokerSecrets MessageBrokerSecrets;
        private readonly IDatabase Cache;

        public Worker(IServiceScopeFactory serviceScopeFactory, IDatabase cache, ILogger<Worker> logger, IOptions<MessageBrokerSettings> messageBrokerSettings, IOptions<MessageBrokerSecrets> messageBrokerSecrets)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
            this.Cache = cache;
            this.Logger = logger;
            this.MessageBrokerSettings = messageBrokerSettings.Value;
            this.MessageBrokerSecrets = messageBrokerSecrets.Value;
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

            this.NotificationRabbitMQChannel = this.RabbitMQConnection.CreateModel();
            this.NotificationRabbitMQChannel.QueueDeclare(this.MessageBrokerSettings.NotificationQueueName, durable: true, exclusive: false, autoDelete: false);
            this.NotificationRabbitMQChannel.BasicQos(this.MessageBrokerSettings.PrefetchSize, this.MessageBrokerSettings.PrefetchCount, false);

            this.TicketCreationRabbitMQChannel = this.RabbitMQConnection.CreateModel();
            this.TicketCreationRabbitMQChannel.QueueDeclare(this.MessageBrokerSettings.TicketCreationQueueName, durable: true, exclusive: false, autoDelete: false);
            this.TicketCreationRabbitMQChannel.BasicQos(this.MessageBrokerSettings.PrefetchSize, this.MessageBrokerSettings.PrefetchCount, false);

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

            #region Notifications
            AsyncEventingBasicConsumer notificationConsumer = new(this.NotificationRabbitMQChannel);
            notificationConsumer.Received += async (bc, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                SendNotificationRequestModel request = message.DeserializeJSON<SendNotificationRequestModel>();
                try
                {
                    // We need services injected to this manager to be in Scoped level (DbContext should be short lived) but always running task creates a single instance of Scoped services. Transient won't work too because we need same DbContext in all the other managers this one calls. So we recreate scope everytime to simulate Scoped injection.
                    using IServiceScope scope = this.ServiceScopeFactory.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<ISendNotificationManager>().SendNotification(request);
                    this.NotificationRabbitMQChannel.BasicAck(ea.DeliveryTag, false);
                    this.Cache.HashDelete(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                }
                catch (Exception e)
                {
                    double retriedCount = this.Cache.HashIncrement(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                    this.Logger.LogError(default, e, e.Message);
                    this.NotificationRabbitMQChannel.BasicNack(ea.DeliveryTag, false, requeue: retriedCount < this.MessageBrokerSettings.RetryCount);
                }
            };

            this.NotificationRabbitMQChannel.BasicConsume(queue: this.MessageBrokerSettings.NotificationQueueName, autoAck: false, consumer: notificationConsumer);
            #endregion

            #region Ticket
            AsyncEventingBasicConsumer ticketConsumer = new(this.TicketCreationRabbitMQChannel);
            ticketConsumer.Received += async (bc, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                CreateTicketRequestModel request = message.DeserializeJSON<CreateTicketRequestModel>();
                try
                {
                    // We need services injected to this manager to be in Scoped level (DbContext should be short lived) but always running task creates a single instance of Scoped services. Transient won't work too because we need same DbContext in all the other managers this one calls. So we recreate scope everytime to simulate Scoped injection.
                    using IServiceScope scope = this.ServiceScopeFactory.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<ITicketCreationBusinessManager>().CreateTicket(request);
                    this.TicketCreationRabbitMQChannel.BasicAck(ea.DeliveryTag, false);
                    this.Cache.HashDelete(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                }
                catch (Exception e)
                {
                    double retriedCount = this.Cache.HashIncrement(this.MessageBrokerSettings.RetryCacheKey, request.MessageID.ToString());
                    this.Logger.LogError(default, e, e.Message);
                    this.TicketCreationRabbitMQChannel.BasicNack(ea.DeliveryTag, false, requeue: retriedCount < this.MessageBrokerSettings.RetryCount);
                }
            };

            this.TicketCreationRabbitMQChannel.BasicConsume(queue: this.MessageBrokerSettings.TicketCreationQueueName, autoAck: false, consumer: ticketConsumer);
            #endregion

            await Task.CompletedTask;
        }
    }
}
