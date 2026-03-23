// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Core.Domain;

public class UserInputModel
{
    public string FromAirport { get; set; } = string.Empty;
    public string ToAirport { get; set; } = string.Empty;
    public string DepartureDate { get; set; } = string.Empty;
    public string Currency { get; set; } = "CAD";
    public string ReturnDate { get; set; } = string.Empty;
    public decimal MaxPrice { get; set; } = decimal.MaxValue;
}
