// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DatabaseRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual required DbSet<FlightInformation> FlightInformation { get; set; }
    public virtual required DbSet<PromptHistory> PromptHistories { get; set; }
}
