// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Infrastructure.DTO;


public class ChatRequest
{
    [JsonProperty("model")]
    public string Model { get; set; } = "claude-sonnet-4-20250514";

    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; } = 1024;

    [JsonProperty("system")]
    public string System { get; set; } = string.Empty;

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; } = new();

    [JsonProperty("tools")]
    public List<Tool> Tools { get; set; } = new();
}
