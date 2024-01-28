using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RJDev.Core.DependencyInjection.Injectable;
using RJDev.Core.DependencyInjection.Tests.Services;
using Xunit;

namespace RJDev.Core.DependencyInjection.Tests
{
    public class ExplicitInjectablesTests
    {
        [Injectable(typeof(ISomeService))]
        private class OnlyExplicitService : ISomeService, ISomeOtherService
        {
            public bool Some => true;
            public bool SomeOther => true;
        }

        [Injectable(typeof(ISomeService))]
        [Injectable(typeof(ISomeOtherService))]
        private class BothExplicitService : ISomeService, ISomeOtherService
        {
            public bool Some => true;
            public bool SomeOther => true;
        }

        [Fact]
        public void OnlyExplicitRegistered()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInjectables(x => x.Implementation == typeof(OnlyExplicitService), Assembly.GetExecutingAssembly());
            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            ISomeService? service1 = provider.GetService<ISomeService>();
            ISomeOtherService? service2 = provider.GetService<ISomeOtherService>();

            Assert.NotNull(service1);
            Assert.IsType<OnlyExplicitService>(service1);

            Assert.Null(service2);
        }

        [Fact]
        public void BothExplicitRegistered()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInjectables(x => x.Implementation == typeof(BothExplicitService), Assembly.GetExecutingAssembly());
            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            ISomeService? service1 = provider.GetService<ISomeService>();
            ISomeOtherService? service2 = provider.GetService<ISomeOtherService>();

            Assert.NotNull(service1);
            Assert.IsType<BothExplicitService>(service1);

            Assert.NotNull(service2);
            Assert.IsType<BothExplicitService>(service2);
        }
    }
}