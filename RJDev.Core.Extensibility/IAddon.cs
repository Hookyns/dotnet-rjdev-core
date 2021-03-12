using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RJDev.Core.Extensibility
{
    public interface IAddon
    {
        /// <summary>
        /// Adds services to the container
        /// </summary>
        /// <param name="serviceCollection">Service collection instance used to create <see cref="IServiceProvider"/></param>
        void ConfigureServices(IServiceCollection serviceCollection);

        /// <summary>
        /// Configure Addon. Called after application initialization.
        /// </summary>
        /// <param name="hostingEnvironment">Hosting environment initialized by the <see cref="IHost" />.</param>
        /// <param name="configuration">Containing the merged configuration of the application and the <see cref="IHost" />.</param>
        /// <param name="serviceProvider">Service provider</param>
        void Configure(IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider);
    }
}