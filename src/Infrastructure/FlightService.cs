// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using Core.Abstractions;
using Core.Domain;
using Infrastructure.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SerpApi;

namespace Infrastructure;

public class FlightService(IOptions<AppSettings> options, ILogger<FlightService> logger) : IFlightService
{
    private readonly AppSettings _appSettings = options.Value;

    public async Task<ResponseObject<FlightOffer[]>> SearchFlightsAsync(UserInputModel input)
    {
        if (DateTime.Parse(input.DepartureDate) < DateTime.Now)
        {
            input.DepartureDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        if (input.WeeksOnvacation > 0)
        {
            input.ReturnDate = DateTime.Parse(input.DepartureDate).AddDays(7 * input.WeeksOnvacation).ToString("yyyy-MM-dd");
        }


        Hashtable ht = new()
            {
                { "engine", "google_flights" },
                { "departure_id", input.FromAirport },
                { "arrival_id", input.ToAirport },
                { "outbound_date", input.DepartureDate.ToString()},
                { "currency", input.Currency },
                { "type", !string.IsNullOrEmpty(input.ReturnDate) ? "1" : "2" }
            };

        if (!string.IsNullOrEmpty(input.ReturnDate))
        {
            ht.Add("return_date", input.ReturnDate);
        }

        try
        {
            GoogleSearch search = new(ht, _appSettings.FlightApiSettings.ApiKey);
            string data = search.GetJson().ToString();

            ResponseFromApi? responseFromApi = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseFromApi>(data);

            if (responseFromApi is null)
            {
                return await Task.FromResult(new ResponseObject<FlightOffer[]>
                {
                    Success = false,
                    Data = null,
                    Message = "Could not find any flights"
                });
            }


            List<FlightOffer> flightOffer = [];

            foreach (var bestFlight in responseFromApi.best_flights)
            {
                foreach (var flight in bestFlight.flights)
                {
                    Layover[]? layovers = [];

                    if (bestFlight.layovers is not null)
                    {
                        layovers = bestFlight.layovers;
                    }

                    FlightOffer fo = new()
                    {
                        Price = bestFlight.price,
                        Airline = flight.airline,
                        FlightNumber = flight.flight_number,
                        Stops = layovers.Length,
                        Currency = "CAD",
                        Itinerary = Newtonsoft.Json.JsonConvert.SerializeObject(layovers),
                        ArrivalTime = DateTime.Parse(flight.arrival_airport.time),
                        DepartureTime = DateTime.Parse(flight.departure_airport.time),
                        FromAirport = input.FromAirport,
                        ToAirport = input.ToAirport,
                        Description = string.Join($"{Environment.NewLine}", flight.extensions)
                    };

                    flightOffer.Add(fo);
                }
            }

            return await Task.FromResult(new ResponseObject<FlightOffer[]>
            {
                Success = true,
                Data = [.. flightOffer]
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while searching for flights.");
            return await Task.FromResult(new ResponseObject<FlightOffer[]>
            {
                Success = false,
                Message = "An error occurred while searching for flights. Please try again later."
            });
        }
    }
}
