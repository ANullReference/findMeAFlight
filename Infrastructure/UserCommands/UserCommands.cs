

using Microsoft.Extensions.Logging;
using Core.Abstractions;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

public class UserCommands(ILogger<UserCommands> _logger) : IUserCommand
{
    public async Task<int> Execute(string[] args)
    {
        CommandApp<UserCommandHandler> app = new();

        app.Configure(config =>
        {
            config.SetApplicationName("look for flights application");
            config.AddCommand<UserCommandHandler>("execute")
                .WithDescription("Executes the user command with the provided arguments.");
        });

        int result = 0;

        try
        {
            result = await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            result = -1; // Return a non-zero exit code to indicate failure
        }

        return result;
    }
}
