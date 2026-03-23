// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Core.Domain;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
    public FlightApiSettings FlightApiSettings { get; set; } = new FlightApiSettings();
    public AiAgentSettings AiAgentSettings { get; set; } = new AiAgentSettings();
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = string.Empty;
}


public class FlightApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
}


public class AiAgentSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string PostLink { get; set; } = string.Empty;
}
