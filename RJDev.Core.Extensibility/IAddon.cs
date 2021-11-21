using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RJDev.Core.Extensibility
{
    public interface IAddon
    {
        /// <summary>
        /// Configure via IHostBuilder.
        /// </summary>
        /// <param name="hostBuilder"></param>
        void Configure(IHostBuilder hostBuilder)
        {
        }

        /// <summary>
        /// Execute the Addon's runtime part.
        /// </summary>
        /// <param name="hostingEnvironment">Hosting environment initialized by the <see cref="IHost" />.</param>
        /// <param name="configuration">Containing the merged configuration of the application and the <see cref="IHost" />.</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="cancellationToken"></param>
        Task Execute(IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}