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
        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// List of addons
        /// </summary>
        private readonly IAddon[] _addons;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="addons"></param>
        /// <param name="hostEnvironment"></param>
        /// <param name="configuration"></param>
        public AddonRunner(IHostEnvironment hostEnvironment, IConfiguration configuration, IServiceProvider serviceProvider, IAddon[] addons)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _addons = addons;
        }

        /// <summary>
        /// Run Addons.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (IAddon addon in _addons)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await addon.Execute(_hostEnvironment, _configuration, _serviceProvider, cancellationToken);
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