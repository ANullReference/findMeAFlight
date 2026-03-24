// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Core.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure;


/// <summary>
/// 
/// </summary>
/// <param name="agentConfig"></param>
/// <param name="logger"></param>
public class CustomMessageHandler(IOptions<AppSettings> options, ILogger<CustomMessageHandler> logger) : DelegatingHandler
{
    private readonly AppSettings _appSettings = options.Value;

    protected override async Task<HttpResponseMessage> SendAsync(
       HttpRequestMessage request,
       CancellationToken cancellationToken)
    {
        try
        {
            request.Headers.Add("x-api-key", _appSettings.AiAgentSettings.ApiKey);
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
