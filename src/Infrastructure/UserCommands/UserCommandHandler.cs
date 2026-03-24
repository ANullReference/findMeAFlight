// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Core.Abstractions;
using Core.Domain;
using Infrastructure.DatabaseRepository.Entities;
using Infrastructure.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console.Cli;

namespace Infrastructure.UserCommands;

public class UserCommandHandler(IOptions<AppSettings> options, AgentConfig agentConfig, ILogger<UserCommandHandler> logger, IFlightService flightService, IHttpClientFactory httpClientFactory) : AsyncCommand<UserInput>
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
        try
        {
            ValidateArgs(settings); // just throw exceptions if validation fails, we will catch them in the main method and log them.

            UserInputModel userInput = settings.ToUserinput();
            int result = 0;

            AppSettings appSettings = options.Value;

            // 1. Search for flights
            ResponseObject<FlightOffer[]> searchResult = await flightService.SearchFlightsAsync(userInput);

            if (!searchResult.Success || searchResult.Data == null)
            {
                logger.LogError("Flight search failed: {Message}", searchResult.Message);
                return -1;
            }

            JsonSerializerSettings jsonSettings = new()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            string flightJson = ToonSharp.ToonSerializer.Serialize(searchResult.Data);
            //string toonSerialized = ToonSharp.ToonSerializer.Serialize(flightJson);

            //todo: connect to ai agent
            //string systemPrompt = agentConfig.Prompts.System;

            HttpClient client = httpClientFactory.CreateClient("AiUrl");
            ChatRequest requestBody = new();

            List<Message> messages = [new() { Role = "user", Content = $"Flights in TOON data format:{flightJson}]" }];

            //requestBody.System = agentConfig.Prompts.System;

            //Set up initial context
            //requestBody.Messages = messages;

            // 2. Build tool list from your agentConfig YAML
            var tools = agentConfig.Tools.Select(t => new Tool
            {
                Name = t.Name,
                Description = t.Description,
                InputSchema = GetSchemaForTool(t.Name)
            }).ToList();

            while (true)
            {
                requestBody = new ChatRequest
                {
                    System = agentConfig.Prompts.System,
                    Tools = tools,
                    Messages = messages
                };

                string requestBodyInString = JsonConvert.SerializeObject(requestBody, jsonSettings);


                logger.LogDebug("Request body:\n{Body}", requestBodyInString);

                HttpContent httpContent = new StringContent(requestBodyInString, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(appSettings.AiAgentSettings.PostLink, httpContent, cancellationToken);

                string raw = await response.Content.ReadAsStringAsync(cancellationToken);
                ClaudeResponse? claudeReply = JsonConvert.DeserializeObject<ClaudeResponse?>(raw);

                if (claudeReply is null)
                {
                    logger.LogError("Null response from Claude.");
                    return -1;
                }

                // Add Claude's reply to history
                messages.Add(new Message { Role = "assistant", Content = claudeReply.Content });

                // Check stop reason
                if (claudeReply.StopReason == "end_turn")
                {
                    var finalText = claudeReply.Content
                        .Where(b => b.Type == "text")
                        .Select(b => b.Text)
                        .FirstOrDefault();

                    logger.LogInformation("Claude final response:\n{Response}", finalText);
                    // TODO: deserialize finalText into your response_shape and save
                    break;
                }

                if (claudeReply.StopReason == "tool_use")
                {
                    List<ToolResultContent> toolResults = [];

                    foreach (var block in claudeReply.Content.Where(b => b.Type == "tool_use"))
                    {
                        string resultJson = block.Name switch
                        {
                            "search_flights" => await HandleSearchFlights(block.Input, jsonSettings),
                            "save_deal" => await HandleSaveDeal(block.Input, jsonSettings),
                            _ => JsonConvert.SerializeObject(new { error = "Unknown tool" })
                        };

                        toolResults.Add(new ToolResultContent
                        {
                            Type = "tool_result",
                            ToolUseId = block.Id,
                            Content = resultJson
                        });
                    }

                    // Feed results back as next user message
                    messages.Add(new Message { Role = "user", Content = toolResults.Cast<object>() });
                }
            }

            return 0;





            //string toolDefinition = string.Format("In Terms of tools you have the following tool: to search 1) '{0}' which uses this as a model input: '{}'", "search_flights", JsonConvert.SerializeObject(new UserInputModel()));

            //todo: give him the context he will be using: You are a shrewd and avid flight finder only looking for the best deals for your customers.
            //You have access to the following tools: 1. SearchFlights(fromAirport, toAirport) - This tool allows you to search for flights between two airports. It returns a list of flight options with details such as airline, price, and departure time. 2. BookFlight(flightOption) - This tool allows you to book a flight option that was returned by the SearchFlights tool. It requires the flight option details as input and returns a confirmation of the booking.

            //todo: Your reasonning will be stored locally for a user to view why you chose certain deals. 

            //todo: Then you will store the deals you find into a local database.
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured. Verify Validation is correct.");
        }

        return await Task.FromResult(-1);
    }

    private async Task<string> HandleSearchFlights(JObject? input, JsonSerializerSettings jsonSettings)
    {
        var refinedInput = new UserInputModel
        {
            FromAirport = input?["from_airport"]?.ToString() ?? string.Empty,
            ToAirport = input?["to_airport"]?.ToString() ?? string.Empty,
            DepartureDate = input?["departure_date"]?.ToString() ?? string.Empty,
            ReturnDate = input?["return_date"]?.ToString() ?? string.Empty,
            Currency = input?["currency"]?.ToString() ?? "CAD",
            MaxPrice = decimal.TryParse(input?["max_price"]?.ToString(), out var mp) ? mp : decimal.MaxValue
        };

        ResponseObject<FlightOffer[]> result = await flightService.SearchFlightsAsync(refinedInput);
        return result.Success
            ? ToonSharp.ToonSerializer.Serialize(result.Data)
            : ToonSharp.ToonSerializer.Serialize(new { error = result.Message });
    }

    private async Task<string> HandleSaveDeal(JObject? input, JsonSerializerSettings jsonSettings)
    {
        if (input is null)
        {
            return string.Empty;
        }

        string arrivalTime = input["arrival_time"]?.ToString() ?? "";
        string departureTime = input["departure_time"]?.ToString() ?? "";


        FlightInformation FlightInformation = new()
        {
            Airline = input["airline"]?.ToString() ?? "",
            ArrivalTime = DateTime.Parse(arrivalTime),
            DepartureTime = DateTime.Parse(departureTime),

        };

        // TODO: wire to your actual DB
        logger.LogInformation("Saving deal: {Deal}", input?.ToString());
        return await Task.FromResult(JsonConvert.SerializeObject(new { success = true }));
    }

    private object GetSchemaForTool(string name) => name switch
    {
        "search_flights" => new
        {
            type = "object",
            properties = new
            {
                from_airport = new { type = "string", description = "IATA code e.g. YUL" },
                to_airport = new { type = "string", description = "IATA code e.g. NRT" },
                departure_date = new { type = "string", description = "yyyy-MM-dd" },
                return_date = new { type = "string", description = "yyyy-MM-dd, empty if one-way" },
                currency = new { type = "string", description = "e.g. CAD, USD" },
                max_price = new { type = "number", description = "Maximum price filter" }
            },
            required = new[] { "from_airport", "to_airport", "departure_date" }
        },
        "save_deal" => new
        {
            type = "object",
            properties = new
            {
                airline = new { type = "string" },
                price = new { type = "number" },
                departure_time = new { type = "string" },
                arrival_time = new { type = "string" },
                stops = new { type = "number" },
                from_airport = new { type = "string" },
                to_airport = new { type = "string" },
                flight_number = new { type = "string" },
                reasoning = new { type = "string", description = "Why this is a good deal" }
            },
            required = new[] { "airline", "price", "departure_time", "arrival_time", "reasoning" }
        },
        _ => new { type = "object", properties = new { } }
    };


    private bool ValidateArgs(UserInput userInput)
    {
        const string dateFormat = "yyyy-MM-dd";

        ArgumentNullException.ThrowIfNull(userInput);
        ArgumentException.ThrowIfNullOrEmpty(userInput.FromAirport, nameof(userInput.FromAirport));
        ArgumentException.ThrowIfNullOrEmpty(userInput.ToAirport, nameof(userInput.ToAirport));
        ArgumentException.ThrowIfNullOrEmpty(userInput.Currency, nameof(userInput.Currency));

        if (!DateTime.TryParseExact(userInput.DepartureDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("Invalid departure date format. Expected format: yyyy-MM-dd", nameof(userInput.DepartureDate));
        }

        if (!string.IsNullOrEmpty(userInput.ReturnDate) && !DateTime.TryParseExact(userInput.ReturnDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("Invalid return date format. Expected format: yyyy-MM-dd", nameof(userInput.ReturnDate));
        }

        return true; // Return true if validation passes, false otherwise
    }
}
