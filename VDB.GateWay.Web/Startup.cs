using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using VDB.Architecture.Web.Core;

namespace VDB.GateWay.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            CommonStartup.CommonServiceConfiguration(new ServiceConfigurationOptions(services, this.Configuration) 
            {
                UseSwagger = true
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            
            services.AddOcelot();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonStartup.CommonAppConfiguration(new AppConfigurationOptions(app, env, "Error"));

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/AuthDocs/swagger/v1/swagger.json", "Auth");
                options.SwaggerEndpoint("/InventoryManagerDocs/swagger/v1/swagger.json", "InventoryManager");
                options.SwaggerEndpoint("/VulnerabilityDetectorDocs/swagger/v1/swagger.json", "VulnerabilityDetector");
                options.SwaggerEndpoint("/NotificationCenterDocs/swagger/v1/swagger.json", "NotificationCenter");
            });

            app.UseCors("CorsPolicy");

            app.UseOcelot().Wait();
        }
    }
}
