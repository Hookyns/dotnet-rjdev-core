using Microsoft.Extensions.DependencyInjection;
using RJDev.Core.DependencyInjection.Injectable;

namespace RJDev.Core.DependencyInjection.Tests.Services
{
    [Injectable(typeof(ISomeService), ServiceLifetime.Transient)]
    class SomeService : ISomeService
    {
    }
}