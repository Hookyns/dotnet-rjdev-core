using System;
using System.Collections.Generic;

namespace RJDev.Core.DependencyInjection.Injectable;

/// <summary>
/// Describes an injectable service, including its implementation type and the service types it provides.
/// </summary>
/// <param name="Implementation"></param>
/// <param name="Services"></param>
public record InjectableDescriptor(
    Type Implementation,
    IEnumerable<InjectableServiceDescriptor> Services
);
