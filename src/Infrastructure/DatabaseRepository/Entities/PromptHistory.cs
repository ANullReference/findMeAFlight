// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Infrastructure.DatabaseRepository.Entities;

public class PromptHistory
{
    public int Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public string Reasoning { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
