using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RJDev.Core.Extensibility
{
    public class AddonRunner : IHostedService
    {
        /// <summary>
        /// Hosting environment
        /// </summary>
        private readonly IHostEnvironment hostEnvironment;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Service provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// List of addons
        /// </summary>
        private readonly IAddon[] addons;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="addons"></param>
        /// <param name="hostEnvironment"></param>
        /// <param name="configuration"></param>
        public AddonRunner(IHostEnvironment hostEnvironment, IConfiguration configuration, IServiceProvider serviceProvider, IAddon[] addons)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
            this.addons = addons;
        }

        /// <summary>
        /// Run Addons.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (IAddon addon in this.addons)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await addon.Execute(this.hostEnvironment, this.configuration, this.serviceProvider, cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception($"'Start' method call of addon '{addon.GetType().FullName}' failed.", ex);
                }
            }
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}