using System;
using System.Collections.Generic;
using Billing.Infrastructure.Data.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Billing.Infrastructure.Extensions;

public static class HealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddMongoDb(
        this IHealthChecksBuilder builder,
        string name,
        IEnumerable<string> tags = default, 
        TimeSpan? timeout = default)
    {
        var hcRegistration = new HealthCheckRegistration(
            name,
            sp => new MongoDbHealthCheck(sp.GetRequiredService<IMongoDatabase>()),
            HealthStatus.Unhealthy,
            tags,
            timeout);

        return builder.Add(hcRegistration);
    }
}