using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using VDB.Architecture.Concern.Options;
using VDB.Architecture.Worker.Core;
using VDB.MicroServices.NotificationCenter.Concern.Options;
using VDB.MicroServices.NotificationCenter.DB.Context;
using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Contract;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Emailing;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Emailing.GoogleMail;
using VDB.MicroServices.NotificationCenter.Manager.Business.Factory;
using VDB.MicroServices.NotificationCenter.Manager.Business.Implementation;
using VDB.MicroServices.NotificationCenter.Manager.Business.Implementation.Notifiers;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Implementation;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;

namespace VDB.MicroServices.NotificationCenter.Worker.MessageConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string extension = environment == "Production" ? ".json" : $".{environment}.json";

            string messageBrokerSettingsFile = $"Configuration/MessageBroker/MessageBrokerSettings{extension}";
            string messageBrokerSecretsFile = $"Configuration/MessageBroker/MessageBrokerSecrets{extension}";
            string redisSettingsFile = $"Configuration/Cache/RedisSettings{extension}";
            string dbSettingsFile = $"Configuration/DB/DBSettings{extension}";
            string emailServiceSettingsFile = $"Configuration/Notification/EmailService/EmailServiceSettings{extension}";
            string emailServiceSecretsFile = $"Configuration/Notification/EmailService/EmailServiceSecrets{extension}";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(messageBrokerSettingsFile, false, true)
                .AddJsonFile(messageBrokerSecretsFile, false, true)
                .AddJsonFile(redisSettingsFile, false, true)
                .AddJsonFile(dbSettingsFile, false, true)
                .AddJsonFile(emailServiceSettingsFile, false, true)
                .AddJsonFile(emailServiceSecretsFile, false, true)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(messageBrokerSettingsFile, false, true);
                    config.AddJsonFile(messageBrokerSecretsFile, false, true);
                    config.AddJsonFile(redisSettingsFile, false, true);
                    config.AddJsonFile(dbSettingsFile, false, true);
                    config.AddJsonFile(emailServiceSettingsFile, false, true);
                    config.AddJsonFile(emailServiceSecretsFile, false, true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    ConfigureOptions(services, config);

                    ConfigureBusinessManagers(services);

                    ConfigureOperations(services);

                    ConfigureFactories(services);

                    ConfigureServiceManagers(services);

                    ConfigureDatabase(services, config);

                    CommonStartup.ConfigureRedis(services, config);

                    services.AddLogging();
                });
        }

        private static void ConfigureOptions(IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<MessageBrokerSettings>().Bind(config.GetSection(nameof(MessageBrokerSettings)));
            services.AddOptions<MessageBrokerSecrets>().Bind(config.GetSection(nameof(MessageBrokerSecrets)));
            services.AddOptions<RedisSettings>().Bind(config.GetSection(nameof(RedisSettings)));
            services.AddOptions<EmailServiceSettings>().Bind(config.GetSection(nameof(EmailServiceSettings)));
            services.AddOptions<EmailServiceSecrets>().Bind(config.GetSection(nameof(EmailServiceSecrets)));
        }

        private static void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(ISendNotificationManager), typeof(SendNotificationManager));
            services.AddScoped(typeof(EmailNotifier));
        }

        private static void ConfigureServiceManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(IEmailServiceManager), typeof(GoogleEmailServiceManager));
        }

        private static void ConfigureOperations(IServiceCollection services)
        {
            services.AddScoped(typeof(INotificationContextOperations), typeof(NotificationContextOperations));
        }

        private static void ConfigureFactories(IServiceCollection services)
        {
            services.AddScoped(typeof(NotifierFactory));
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<NotificationCenterDataContext>(options => options.UseSqlServer(config.GetConnectionString("NotificationCenterDataContext"), sqlServerOptions => sqlServerOptions.MigrationsAssembly("VDB.MicroServices.NotificationCenter.DB.Context")));
            services.AddScoped(typeof(INotificationCenterUnitOfWork), typeof(NotificationCenterUnitOfWork));
        }
    }
}
