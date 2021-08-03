using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDB.Architecture.Web.Core;
using VDB.MicroServices.NotificationCenter.DB.Context;
using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Manager.Business.Implementation;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Manager.Mapper._AutoMapperProfiles;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Implementation;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;

namespace VDB.MicroServices.NotificationCenter.Web.API
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
                UseSwagger = true,
                AutoMapperProfile = new NotificationCenterMappingProfile()
            });

            ConfigureDatabase(services);

            ConfigureBusinessManagers(services);
            
            ConfigureOperations(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonStartup.CommonAppConfiguration(new AppConfigurationOptions(app, env, "Error")
            {
                SwaggerSettings = new()
                {
                    APIName = "NotificationCenter.Web.API",
                    SwaggerEndpoint = "/swagger/v1/swagger.json",
                    Server = "/NotificationCenter"
                }
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<NotificationCenterDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NotificationCenterDataContext"), sqlServerOptions => sqlServerOptions.MigrationsAssembly("VDB.MicroServices.NotificationCenter.DB.Context")));
            services.AddScoped(typeof(INotificationCenterUnitOfWork), typeof(NotificationCenterUnitOfWork));
        }

        private void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(INotificationContextManager), typeof(NotificationContextManager));
        }

        private void ConfigureOperations(IServiceCollection services)
        {
            services.AddScoped(typeof(INotificationContextOperations), typeof(NotificationContextOperations));
            services.AddScoped(typeof(INotificationTypeOperations), typeof(NotificationTypeOperations));
            services.AddScoped(typeof(INotificationAudienceOperations), typeof(NotificationAudienceOperations));
        }
    }
}
