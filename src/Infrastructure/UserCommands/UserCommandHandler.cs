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
            //todo: connect to ai agent

            //todo: give him the context he will be using: You are a shrewd and avid flight finder only looking for the best deals for your customers.
            //You have access to the following tools: 1. SearchFlights(fromAirport, toAirport) - This tool allows you to search for flights between two airports. It returns a list of flight options with details such as airline, price, and departure time. 2. BookFlight(flightOption) - This tool allows you to book a flight option that was returned by the SearchFlights tool. It requires the flight option details as input and returns a confirmation of the booking.

            //todo: Your reasonning will be stored locally for a user to view why you chose certain deals. 

            //todo: Then you will store the deals you find into a local database.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured when generating build request manifest");
        }

        return await Task.FromResult(-1);
    }
}
