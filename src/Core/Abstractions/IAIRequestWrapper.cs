// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Core.Abstractions;

public interface IAIRequestWrapper : IAsyncDisposable
{
    Task<bool> Connect();
    Task<string> InitializeContext(string context);
    Task<string> SendPrompt(string request);
    Task<StringBuilder> GetHistoryOfSentPromts(string request);
}
