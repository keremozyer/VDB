using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace VDB.Architecture.Web.Core
{
    public record AppConfigurationOptions(IApplicationBuilder App, IWebHostEnvironment HostEnvironment, string ErrorHandlerEndpoint)
    {
        public SwaggerSettings SwaggerSettings { get; set; }
    }

    public class SwaggerSettings
    {
        public string SwaggerEndpoint { get; set; }
        public string APIName { get; set; }
        public string Server { get; set; }
    }
}
