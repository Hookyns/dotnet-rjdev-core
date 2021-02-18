using System;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable
{
    /// <summary>
    /// Attribute marking default implementations of services which should not be registered manually.
    /// Each registration can be overriden by different implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]  
    public class InjectableAttribute : Attribute
    {
        /// <summary>
        /// Service (dependency) type
        /// </summary>
        public Type Service { get; }
        
        /// <summary>
        /// Service lifetime
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceLifetime"></param>
        public InjectableAttribute(Type service, ServiceLifetime serviceLifetime)
        {
            this.Service = service;
            this.ServiceLifetime = serviceLifetime;
        }
    }
}