using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDB.Architecture.Concern.Options;
using VDB.Architecture.Web.Core;
using VDB.MicroServices.Auth.Concern.Options;
using VDB.MicroServices.Auth.Manager.Business.Implementation;
using VDB.MicroServices.Auth.Manager.Business.Interface;

namespace VDB.MicroServices.Auth.Web.API
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
            ConfigureOptions(services);

            CommonStartup.CommonServiceConfiguration(new ServiceConfigurationOptions(services, this.Configuration) 
            {
                UseSwagger = true
            });

            ConfigureBusinessManagers(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonStartup.CommonAppConfiguration(new AppConfigurationOptions(app, env, "Error") 
            {
                SwaggerSettings = new()
                {
                    APIName = "Auth.Web.API",
                    SwaggerEndpoint = "/swagger/v1/swagger.json",
                    Server = "/Auth"
                }
            });
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions<TokenSettings>().Bind(this.Configuration.GetSection(nameof(TokenSettings)));
            services.AddOptions<LDAPSettings>().Bind(this.Configuration.GetSection(nameof(LDAPSettings)));
            services.AddOptions<LDAPSecrets>().Bind(this.Configuration.GetSection(nameof(LDAPSecrets)));
        }

        private void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(ITokenManager), typeof(TokenManager));
            services.AddScoped(typeof(IAuthManager), typeof(LDAPManager));
        }
    }
}
