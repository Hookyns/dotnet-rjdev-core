using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RJDev.Core.Extensibility.Tests.TestAddonProj
{
    public class TestAddon : IAddon
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISomeService, SomeService>();
        }

        public void Configure(IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<ISomeService>().ConfigureCalled = true;
        }
    }
}