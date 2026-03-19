// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Infrastructure.DatabaseRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public virtual DbSet<FlightInformation> FlightInformation { get; set; }
    public virtual DbSet<PromptHistory> PromptHistories { get; set; }


}
