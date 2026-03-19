using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;


namespace Infrastructure.UserCommands.Extensions;

public sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    private readonly IServiceCollection _services = services;

    public void Register(Type service, Type implementation) => _services.AddSingleton(service, implementation);
    public void RegisterInstance(Type service, object implementation) => _services.AddSingleton(service, implementation);
    public void RegisterLazy(Type service, Func<object> factory) => _services.AddSingleton(service, _ => factory());
    public ITypeResolver Build() => new TypeResolver(_services.BuildServiceProvider());
}
