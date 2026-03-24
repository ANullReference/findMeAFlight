// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Infrastructure.DTO;

public class ToolResultContent
{
    [JsonProperty("type")] public string Type { get; set; } = "tool_result";
    [JsonProperty("tool_use_id")] public string ToolUseId { get; set; } = string.Empty;
    [JsonProperty("content")] public string Content { get; set; } = string.Empty;
}
