// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Core.Domain;
using Infrastructure.DTO;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

public class UserCommandHandler(ILogger<UserCommandHandler> _logger) : AsyncCommand<UserInput>
{
    public async override Task<int> ExecuteAsync(CommandContext context, UserInput settings, CancellationToken cancellationToken)
    {
        UserInputModel userInput = settings.ToUserinput();
        int result = 0;

        try
        {
            // result = await _submitBuildRequest.Execute(userInput, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured when generating build request manifest");
        }

        return await Task.FromResult(-1);
    }
}
