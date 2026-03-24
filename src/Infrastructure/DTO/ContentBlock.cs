// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.DTO;

public class ContentBlock
{
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("text")] public string? Text { get; set; }
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("input")] public JObject? Input { get; set; }
}
