using System;
using Microsoft.Extensions.DependencyInjection;
using RJDev.Core.DependencyInjection.Injectable;
using RJDev.Core.DependencyInjection.Tests.Services;
using Xunit;

namespace RJDev.Core.DependencyInjection.Tests
{
    public class InjectableRegistrarTest
    {
        /// <summary>
        /// Registration and resolve test
        /// </summary>
        [Fact]
        public void RegisterAndResolve()
        {
            IServiceProvider provider = GetServiceProvider();
            ISomeService someService = provider.GetService<ISomeService>();
            
            Assert.NotNull(someService);
            Assert.IsType<SomeService>(someService);
        }

        /// <summary>
        /// Prepare and return IServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.WithInjectablesFrom(typeof(InjectableRegistrarTest).Assembly);

            IServiceProvider provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}