// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Core.Domain;
using Spectre.Console.Cli;

namespace Infrastructure.DTO;

public class UserInput : CommandSettings
{

    [CommandOption("-f|--from")]
    [Description("From Flight")]
    public string FromAirport { get; set; } = string.Empty;

    [CommandOption("-t|--to")]
    [Description("To Flight")]
    public string ToAirport { get; set; } = string.Empty;


    public UserInputModel ToUserinput()
    {
        return new UserInputModel
        {
            FromAirport = this.FromAirport,
            ToAirport = this.ToAirport
        };
    }
}
