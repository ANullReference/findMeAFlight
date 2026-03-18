// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using Infrastructure.DTO;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

public class UserCommandHandler : AsyncCommand<UserInputDto>
{
    public override Task<int> ExecuteAsync(CommandContext context, UserInputDto settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
