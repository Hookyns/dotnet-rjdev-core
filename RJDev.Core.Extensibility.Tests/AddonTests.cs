using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RJDev.Core.Extensibility.Tests.TestAddonProj.Services;
using Xunit;

namespace RJDev.Core.Extensibility.Tests
{
    public class AddonTests
    {
        [Fact]
        public void AddonConfigureServices()
        {
            Host.CreateDefaultBuilder()
                .WithAddons()
                .ConfigureServices((_, serviceCollection) =>
                {
                    Assert.Contains(serviceCollection, x => x.ServiceType == typeof(ISomeService) && x.ImplementationType == typeof(SomeService));
                });
        }
        
        [Fact]
        public async Task AddonConfigure()
        {
            using IHost host = Host.CreateDefaultBuilder()
                .WithAddons()
                .UseConsoleLifetime()
                .Build();
            
                await host.StartAsync();

                // Service from addon registered and resolved
                ISomeService someService = host.Services.GetRequiredService<ISomeService>();
                Assert.NotNull(someService);
                
                // Configure method called
                Assert.True(someService.ConfigureCalled);
        }
    }
}