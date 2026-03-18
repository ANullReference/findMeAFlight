// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Infrastructure.Extensions;

public static class FindMeAFlightExtensions
{
    public static int UseMidnightCommands<TDefaultCommand>(this IServiceCollection services, string[] args)
       where TDefaultCommand : class, ICommand
    {
        TypeRegistrar registrar = new(services);
        CommandApp<TDefaultCommand> app = new(registrar);

        // This handles the async wrapper and returns the exit code
        return app.Run(args);
    }
}
