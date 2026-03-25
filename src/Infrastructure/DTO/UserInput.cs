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
    [Description("Departure airport IATA code (e.g. YUL)")]
    public string FromAirport { get; set; } = string.Empty;

    [CommandOption("-t|--to")]
    [Description("Arrival airport IATA code (e.g. NRT)")]
    public string ToAirport { get; set; } = string.Empty;

    [CommandOption("-d|--date")]
    [Description("Departure date (yyyy-MM-dd)")]
    public string DepartureDate { get; set; } = string.Empty;

    [CommandOption("-r|--return")]
    [Description("Return date (yyyy-MM-dd) — omit for one-way")]
    public string? ReturnDate { get; set; } = string.Empty;

    [CommandOption("-w|--weeks")]
    [Description("Weeks on vacation")]
    public int? WeeksOnvacation { get; set; } = 0;


    [CommandOption("-m|--max-price")]
    [Description("Alert threshold — only surface deals below this price")]
    public decimal MaxPrice { get; set; } = decimal.MaxValue;

    [CommandOption("-c|--currency")]
    [Description("Currency code (default: CAD)")]
    public string Currency { get; set; } = "CAD";

    public UserInputModel ToUserinput()
    {
        //if (WeeksOnvacation > 0)
        //{ 
        //    ReturnDate = 
        //}

        return new UserInputModel
        {
            FromAirport = this.FromAirport,
            ToAirport = this.ToAirport,
            Currency = this.Currency,
            DepartureDate = this.DepartureDate,
            ReturnDate = this.ReturnDate,
            MaxPrice = this.MaxPrice,
            WeeksOnvacation = this.WeeksOnvacation ?? 0,
        };
    }
}
