using Core;
using Core.Abstractions;
using Infrastructure.UserCommands;
using Infrastructure.UserCommands.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console.Cli;

class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        TypeRegistrar registrar = null;

        using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            registrar = new TypeRegistrar(services);
            services.AddSingleton<IServiceManager, ServiceManager>();
            services.AddScoped<IUserCommand, UserCommand>();
        })
        .UseSerilog()
        .Build();


        int result = 0;

        try
        {
            CommandApp<UserCommandHandler> app = new(registrar);
            result = await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while executing the application.");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
            Environment.Exit(result);
        }
    }
}
