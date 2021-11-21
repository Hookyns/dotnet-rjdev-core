using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RJDev.Core.Extensibility.Tests.TestAddonProj.Services;

namespace RJDev.Core.Extensibility.Tests.TestAddonProj
{
    public class TestAddon : IAddon
    {
        public void Configure(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddSingleton<ISomeService, SomeService>();
            });
        }

        public Task Execute(IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            serviceProvider.GetRequiredService<ISomeService>().ConfigureCalled = true;
            return Task.CompletedTask;
        }
    }
}