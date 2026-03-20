// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Core.Domain;
using Infrastructure.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

/*
 

The app: findMeAFlight — C# console app that searches for flights and alerts you when a price is interesting.
Tech stack:

.NET 10, Console app
Spectre.Console for CLI
EF Core + SQL Server
Serilog
Amadeus API for flight data (free tier, ~2000 calls/month, you'll use ~150)
Anthropic/Claude as the AI agent brain

Architecture — two projects:

Core — ServiceManager, UserInputModel, ResponseObject<T>, AppSettings
Infrastructure — Program.cs, UserCommandHandler, ApplicationDbContext, FlightInformation, PromptHistory, Spectre.Console wiring

Where you left off:

AppSettings needs AmadeusSettings added (BaseUrl, ClientId, ClientSecret)
Config strategy agreed: appsettings.json → appsettings.local.json → appsettings.prod.json, driven by DOTNET_ENVIRONMENT
IServiceManager interface doesn't exist yet — blocking
AmadeusService not built yet — next big piece
UserInput needs DepartureDate, ReturnDate?, MaxPrice? added

Next coding steps in order:

Update AppSettings with Amadeus config
Create IServiceManager + IAmadeusService in Core.Abstractions
Build AmadeusService — OAuth2 token caching + flight search
Update UserInput / UserInputModel with date and price fields
Wire Claude tool-use loop into UserCommandHandler
Persist flights + reasoning to DB

Test vs Prod Amadeus: Same endpoints, swap base URL and credentials. Test has real data but non-live prices. Prod requires a quick approval. Just an env var swap.

See you tomorrow.
 
 */


public class UserCommandHandler(IOptions<AppSettings> options, ILogger<UserCommandHandler> logger) : AsyncCommand<UserInput>
{
    /// <summary>
    /// Executes the flight deal search and booking operation asynchronously using the provided command context and user
    /// input settings.
    /// </summary>
    /// <remarks>The method processes user input to search for flight deals and attempts to book the best
    /// available options. Reasoning and selected deals are stored locally for user review. If an error occurs during
    /// execution, the method logs the error and returns -1.</remarks>
    /// <param name="context">The context for the command execution, containing information relevant to the current operation.</param>
    /// <param name="settings">The user input settings that specify search criteria and preferences for flight deals.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is an integer indicating the outcome of the
    /// execution.</returns>
    public async override Task<int> ExecuteAsync(CommandContext context, UserInput settings, CancellationToken cancellationToken)
    {
        UserInputModel userInput = settings.ToUserinput();
        int result = 0;

        try
        {
            AppSettings appSettings = options.Value;
            //todo: connect to ai agent

            //todo: give him the context he will be using: You are a shrewd and avid flight finder only looking for the best deals for your customers.
            //You have access to the following tools: 1. SearchFlights(fromAirport, toAirport) - This tool allows you to search for flights between two airports. It returns a list of flight options with details such as airline, price, and departure time. 2. BookFlight(flightOption) - This tool allows you to book a flight option that was returned by the SearchFlights tool. It requires the flight option details as input and returns a confirmation of the booking.

            //todo: Your reasonning will be stored locally for a user to view why you chose certain deals. 

            //todo: Then you will store the deals you find into a local database.
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured when generating build request manifest");
        }

        return await Task.FromResult(-1);
    }
}
