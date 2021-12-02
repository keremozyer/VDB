using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.Manager.Business.Interface;

namespace VDB.MicroServices.CVEData.Worker.Downloader
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly ILogger<Worker> Logger;
        private System.Timers.Timer Timer;
        private readonly CVEDownloaderSettings CVEDownloaderSettings;

        public Worker(IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger, IOptions<CVEDownloaderSettings> cveDownloaderSettings)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
            this.Logger = logger;
            this.CVEDownloaderSettings = cveDownloaderSettings.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Download().Wait(cancellationToken);

            this.Timer = new(this.CVEDownloaderSettings.DownloadFrequency);
            this.Timer.Elapsed += async (sender, e) => await Download();
            this.Timer.Start();

            return base.StartAsync(cancellationToken);
        }

        public async Task Download()
        {
            try
            {
                // We need services injected to this manager to be in Scoped level (DbContext should be short lived) but always running task creates a single instance of Scoped services. Transient won't work too because we need same DbContext in all the other managers this one calls. So we recreate scope everytime to simulate Scoped injection.
                using IServiceScope scope = this.ServiceScopeFactory.CreateScope();
                await scope.ServiceProvider.GetRequiredService<ICVEDownloadBusinessManager>().GetSystemUpToDate();
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, e.Message);
                throw;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
