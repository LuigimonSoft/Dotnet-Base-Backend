using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Integration.Test.Utilities
{
    public class CustomWebApplicationFactory<TStarup>: WebApplicationFactory<TStarup>, IDisposable where TStarup : class
    {
        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");
            builder.ConfigureServices(services =>
            {
                //services.AddSingleton<IBaseService, BaseService>();
            });
            base.ConfigureWebHost(builder);
        }
    }
}
