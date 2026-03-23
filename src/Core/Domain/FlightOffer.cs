// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Core.Domain;

public class FlightOffer
{
    public string Airline { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int Stops { get; set; } = 0;
    public string Itinerary { get; set; } = string.Empty;

    public string FlightNumber { get; set; } = string.Empty;
}
