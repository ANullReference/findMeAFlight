// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Infrastructure.DTO;

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [JsonProperty("content")]
    public object Content { get; set; } = string.Empty;


    //[JsonProperty("raw_content")]
    //public List<object> RawContent { get; set; } = [];
}
