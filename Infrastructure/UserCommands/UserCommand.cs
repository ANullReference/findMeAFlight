// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Core.Abstractions;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

public class UserCommand(ILogger<UserCommand> _logger) : IUserCommand
{
    public async Task<int> Execute(string[] args)
    {
        CommandApp<UserCommandHandler> app = new CommandApp<UserCommandHandler>();

        app.Configure(config =>
        {
            config.SetApplicationName("find me a flight");
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
