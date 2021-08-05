using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.ExternalData.Manager.Contract;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.CVEData;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.CVEData.NVD;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.VulnerabilityReport;
using VDB.MicroServices.CVEData.Manager.Business.Implementation;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Implementation;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;

namespace VDB.MicroServices.CVEData.Worker.Downloader
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

            string cveDownloaderSettingsFile = $"Configuration/CVEData/CVEDownloaderSettings{extension}";
            string cveStorageSettingsFile = $"Configuration/CVEData/CVEStorageSettings{extension}";
            string dbSettingsFile = $"Configuration/DB/DBSettings{extension}";
            string endpointSettingsFile = $"Configuration/ExternalServices/EndpointSettings{extension}";
            
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(cveDownloaderSettingsFile, false, true)
                .AddJsonFile(cveStorageSettingsFile, false, true)
                .AddJsonFile(dbSettingsFile, false, true)
                .AddJsonFile(endpointSettingsFile, false, true)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(cveDownloaderSettingsFile, false, true);
                    config.AddJsonFile(cveStorageSettingsFile, false, true);
                    config.AddJsonFile(dbSettingsFile, false, true);
                    config.AddJsonFile(endpointSettingsFile, false, true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    ConfigureOptions(services, config);

                    ConfigureDatabase(services, config);

                    ConfigureBusinessManagers(services);

                    ConfigureOperations(services);

                    ConfigureHttpClients(services);

                    ConfigureServiceManagers(services);

                    services.AddLogging();
                });
        }

        private static void ConfigureOptions(IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<CVEDownloaderSettings>().Bind(config.GetSection(nameof(CVEDownloaderSettings)));
            services.AddOptions<CVEStorageSettings>().Bind(config.GetSection(nameof(CVEStorageSettings)));
            services.AddOptions<EndpointSettings>().Bind(config.GetSection(nameof(EndpointSettings)));
        }

        private static void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(ICVEDataStorageBusinessManager), typeof(CVEDataStorageBusinessManager));
            services.AddScoped(typeof(ICVEDownloadBusinessManager), typeof(CVEDownloadBusinessManager));
            services.AddScoped(typeof(IProductBusinessManager), typeof(ProductBusinessManager));
            services.AddScoped(typeof(IVendorBusinessManager), typeof(VendorBusinessManager));
        }

        private static void ConfigureServiceManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(ICVEServiceManager), typeof(NVDServiceManager));
            services.AddScoped(typeof(IVulnerabilityReportServiceManager), typeof(VulnerabilityReportServiceManager));
        }

        private static void ConfigureOperations(IServiceCollection services)
        {
            services.AddScoped(typeof(ICPEOperations), typeof(CPEOperations));
            services.AddScoped(typeof(ICVEOperations), typeof(CVEOperations));
            services.AddScoped(typeof(ICVENodeOperations), typeof(CVENodeOperations));
            services.AddScoped(typeof(ICVEDownloadLogOperations), typeof(CVEDownloadLogOperations));
            services.AddScoped(typeof(IProductOperations), typeof(ProductOperations));
            services.AddScoped(typeof(IVendorOperations), typeof(VendorOperations));
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CVEDataDataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString(nameof(CVEDataDataContext)), sqlServerOptions => sqlServerOptions.MigrationsAssembly("VDB.MicroServices.CVEData.DB.Context"));
            });
            services.AddScoped(typeof(ICVEDataUnitOfWork), typeof(CVEDataUnitOfWork));
        }

        private static void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient<ICVEServiceManager, NVDServiceManager>();
            services.AddHttpClient<IVulnerabilityReportServiceManager, VulnerabilityReportServiceManager>();
        }
    }
}
