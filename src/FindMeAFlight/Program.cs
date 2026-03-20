// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Core;
using Core.Abstractions;
using Core.Domain;
using Infrastructure.DatabaseRepository;
using Infrastructure.UserCommands;
using Infrastructure.UserCommands.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "prod"}.json", optional: true)
            .Build();

        using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            registrar = new TypeRegistrar(services);
            services.AddSingleton<IServiceManager, ServiceManager>();
            services.AddScoped<IUserCommand, UserCommand>();
            services.Configure<AppSettings>(config);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]));
        })
        .UseSerilog()
        .Build();

        int result = 0;

        //init database
        bool dbCreated = await host.Services.GetRequiredService<ApplicationDbContext>().Database.EnsureCreatedAsync();

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
