using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Gelf.Extensions.Logging;
using System;

namespace VDB.MicroServices.NotificationCenter.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    string environment = hostingContext.HostingEnvironment.EnvironmentName;
                    string extension = environment == "Production" ? ".json" : $".{environment}.json";

                    config.AddJsonFile($"Configuration/DB/DBSettings{extension}");
                    config.AddJsonFile($"Configuration/Cache/RedisSettings{extension}", false, true);
                    config.AddJsonFile($"Configuration/App/AppSettings{extension}", false, true);
                    config.AddJsonFile($"Configuration/Auth/TokenSettings{extension}", false, true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureLogging((context, builder) => builder.AddGelf(options =>
                        {
                            options.AdditionalFields["machine_name"] = Environment.MachineName;
                        }));
                });
        }
    }
}
