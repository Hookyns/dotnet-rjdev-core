using System;
using System.Collections.Generic;

namespace RJDev.Core.DependencyInjection.Injectable;

public record InjectableDescriptor(Type Implementation, IEnumerable<InjectableServiceDescriptor> Services);