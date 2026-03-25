// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DatabaseRepository.Entities;

public class FlightInformation
{
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string Airline { get; set; } = string.Empty;
    public string DepartureAirport { get; set; } = string.Empty;
    public string ArrivalAirport { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 6)")] // Explicitly sets the SQL column type
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int LayOver { get; set; }
    public string Itinerary { get; set; } = string.Empty; // JSON string representing the itinerary details [MTL to van datetime], [van to japan datetime etc]
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTimeUTC { get; set; }
    public DateTime ArrivalTimeUTC { get; set; }
    public DateTime Timestamp { get; set; }

    public DateTime? ReturnDate { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string Reasoning { get; set; } = string.Empty;
}
