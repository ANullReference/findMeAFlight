// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Spectre.Console.Cli;
using Core.Domain;
using Spectre.Console;
namespace Infrastructure.DTO;

public class UserInputDto : CommandSettings
{
    [CommandOption("-f|--from")]
    [Description("From location")]
    public string FromAirport { get; set; } = string.Empty;

    [CommandOption("-t|--to")]
    [Description("To location")]
    public string ToAirport { get; set; } = string.Empty;

    public UserInputModel ToUserInput()
    {
        return new()
        {
            FromAirport = this.FromAirport,
            ToAirport = this.ToAirport
        };
    }

    public static UserInputDto FromUserInputModel(UserInputModel model)
    {
        return new()
        {
            FromAirport = model.FromAirport,
            ToAirport = model.ToAirport
        };
    }   
}
