using System;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable;

public record InjectableServiceDescriptor(Type Service, ServiceLifetime? ServiceLifetime);