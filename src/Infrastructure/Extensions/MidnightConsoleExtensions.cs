using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands.Extensions;

public static class MidnightConsoleExtensions
{
    public static int UseFindMeAFlightCommands<TDefaultCommand>(this IServiceCollection services, string[] args)
        where TDefaultCommand : class, ICommand
    {
        TypeRegistrar registrar = new (services);
        CommandApp<TDefaultCommand> app = new (registrar);

        // This handles the async wrapper and returns the exit code
        return app.Run(args);
    }
}
