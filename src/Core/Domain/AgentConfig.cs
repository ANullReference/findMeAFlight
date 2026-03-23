// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Core.Domain;


public class AgentConfig
{
    public string Version { get; set; } = string.Empty;
    public ModelConfig Models { get; set; } = new ModelConfig();
    public PromptConfig Prompts { get; set; } = new PromptConfig();
    public ToolConfig ToolConfig { get; set; } = new ToolConfig();
}


public class ModelConfig
{
    public string Default { get; set; } = string.Empty;
    public string Fast { get; set; } = string.Empty;
}

public class PromptConfig
{
    public string System { get; set; } = string.Empty;
    public string ResponseShape { get; set; } = string.Empty;
    public string UserTemplate { get; set; } = string.Empty;
}

public class ToolConfig
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
