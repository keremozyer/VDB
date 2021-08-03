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
        private readonly ILogger<Worker> Logger;
        private System.Timers.Timer Timer;
        private readonly CVEDownloaderSettings CVEDownloaderSettings;
        private readonly ICVEDownloadBusinessManager CVEDownloadBusinessManager;

        public Worker(ILogger<Worker> logger, IOptions<CVEDownloaderSettings> cveDownloaderSettings, ICVEDownloadBusinessManager cveDownloadBusinessManager)
        {
            this.Logger = logger;
            this.CVEDownloaderSettings = cveDownloaderSettings.Value;
            this.CVEDownloadBusinessManager = cveDownloadBusinessManager;
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
                await this.CVEDownloadBusinessManager.GetSystemUpToDate();
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
