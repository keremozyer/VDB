using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDB.Architecture.Web.Core;
using VDB.MicroServices.CVEData.Concern.Options;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.ExternalData.Manager.Contract;
using VDB.MicroServices.CVEData.ExternalData.Manager.Service.CVEData.NVD;
using VDB.MicroServices.CVEData.Manager.Business.Implementation;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Mapper._AutoMapperProfiles;
using VDB.MicroServices.CVEData.Manager.Operation.Implementation;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;

namespace VDB.MicroServices.CVEData.Web.API
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
                AutoMapperProfile = new CVEDataMappingProfile()
            });

            ConfigureDatabase(services);

            ConfigureBusinessManagers(services);
            ConfigureOperations(services);

            services.AddHttpClient<ICVEServiceManager, NVDServiceManager>();

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonStartup.CommonAppConfiguration(new AppConfigurationOptions(app, env, "Error"));
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions<EndpointSettings>().Bind(this.Configuration.GetSection(nameof(EndpointSettings)));
            services.AddOptions<CVESearchSettings>().Bind(this.Configuration.GetSection(nameof(CVESearchSettings)));
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<CVEDataDataContext>(options => 
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(CVEDataDataContext)), sqlServerOptions => sqlServerOptions.MigrationsAssembly("VDB.MicroServices.CVEData.DB.Context"));
            });
            services.AddScoped(typeof(ICVEDataUnitOfWork), typeof(CVEDataUnitOfWork));
        }

        private void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(ICVESearchBusinessManager), typeof(CVESearchBusinessManager));
        }

        private void ConfigureOperations(IServiceCollection services)
        {
            services.AddScoped(typeof(ICPEOperations), typeof(CPEOperations));
            services.AddScoped(typeof(ICVENodeOperations), typeof(CVENodeOperations));
        }
    }
}
