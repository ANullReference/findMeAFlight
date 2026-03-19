using Spectre.Console.Cli;

namespace Infrastructure.UserCommands.Extensions;

public sealed class TypeResolver(IServiceProvider serviceProvider) : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public object? Resolve(Type? type) => type != null ? _serviceProvider.GetService(type) : null;
    public void Dispose() { if (_serviceProvider is IDisposable d) d.Dispose(); }
}
