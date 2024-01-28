using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RJDev.Core.DependencyInjection.Injectable;
using RJDev.Core.DependencyInjection.Tests.Services;
using Xunit;

namespace RJDev.Core.DependencyInjection.Tests
{
    public class ImplicitInjectablesTests
    {
        [Injectable]
        private class FooService : ISomeService, ISomeOtherService
        {
            public bool Some => true;
            public bool SomeOther => true;
        }

        [Fact]
        public void BothInterfacesRegistered()
        {
            IServiceProvider provider = GetServiceProvider();

            ISomeService? someService = provider.GetService<ISomeService>();
            ISomeOtherService? someOtherService = provider.GetService<ISomeOtherService>();

            Assert.NotNull(someService);
            Assert.IsType<FooService>(someService);

            Assert.NotNull(someOtherService);
            Assert.IsType<FooService>(someOtherService);
        }

        [Fact]
        public void IsTransientByDefault()
        {
            IServiceProvider provider = GetServiceProvider();

            ISomeService? someService = provider.GetService<ISomeService>();
            ISomeService? someService2 = provider.GetService<ISomeService>();

            Assert.True(someService != someService2);
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInjectables(x => x.Implementation == typeof(FooService), Assembly.GetExecutingAssembly());
            return serviceCollection.BuildServiceProvider();
        }

        private abstract class ParentServiceBase : ISomeOtherService
        {
            public bool SomeOther => true;
        }

        [Injectable]
        private class ChildService : ParentServiceBase, ISomeService
        {
            public bool Some => true;
        }

        [Fact]
        public void InheritedInterfacesRegistered()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInjectables(x => x.Implementation == typeof(ChildService), Assembly.GetExecutingAssembly());
            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            ISomeService? someService = provider.GetService<ISomeService>();
            ISomeOtherService? someOtherService = provider.GetService<ISomeOtherService>();

            Assert.NotNull(someService);
            Assert.IsType<ChildService>(someService);

            Assert.NotNull(someOtherService);
            Assert.IsType<ChildService>(someOtherService);
        }
    }
}