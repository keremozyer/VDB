using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDB.Architecture.Web.Core;
using VDB.MicroServices.InventoryManager.DB.Context;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Implementation;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Manager.Mapper._AutoMapperProfiles;
using VDB.MicroServices.InventoryManager.Manager.Operation.Implementation;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;

namespace VDB.MicroServices.InventoryManager.Web.API
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
                AutoMapperProfile = new InventoryManagerMappingProfile()
            });

            ConfigureDatabase(services);

            ConfigureBusinessManagers(services);
            ConfigureOperations(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonStartup.CommonAppConfiguration(new AppConfigurationOptions(app, env, "Error")
            {
                SwaggerSettings = new()
                {
                    APIName = "InventoryManager.Web.API",
                    SwaggerEndpoint = "/swagger/v1/swagger.json",
                    Server = "/InventoryManager"
                }
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<InventoryManagerDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("InventoryManagerDataContext"), sqlServerOptions => sqlServerOptions.MigrationsAssembly("VDB.MicroServices.InventoryManager.DB.Context")));
            services.AddScoped(typeof(IInventoryManagerUnitOfWork), typeof(InventoryManagerUnitOfWork));
        }

        private void ConfigureBusinessManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(IProductBusinessManager), typeof(ProductBusinessManager));
            services.AddScoped(typeof(IProductVersionBusinessManager), typeof(ProductVersionBusinessManager));
            services.AddScoped(typeof(IServerBusinessManager), typeof(ServerBusinessManager));
            services.AddScoped(typeof(IVendorBusinessManager), typeof(VendorBusinessManager));
        }

        private void ConfigureOperations(IServiceCollection services)
        {
            services.AddScoped(typeof(IProductOperations), typeof(ProductOperations));
            services.AddScoped(typeof(IProductVersionOperations), typeof(ProductVersionOperations));
            services.AddScoped(typeof(IServerOperations), typeof(ServerOperations));
            services.AddScoped(typeof(IVendorOperations), typeof(VendorOperations));
        }
    }
}
