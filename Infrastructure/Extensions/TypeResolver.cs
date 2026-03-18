// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;

namespace Infrastructure.Extensions;


public sealed class TypeResolver(IServiceProvider _serviceProvider) : ITypeResolver, IDisposable
{
    public object? Resolve(Type? type) => type != null ? _serviceProvider.GetService(type) : null;
    public void Dispose() { if (_serviceProvider is IDisposable d) d.Dispose(); }
}
