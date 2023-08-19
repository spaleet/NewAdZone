﻿using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Plan.Domain.Entities;

namespace Plan.Infrastructure.Context;

public class PlanDbContext : MongoDbContext
{
    public PlanDbContext(IOptions<MongoOptions> options) : base(options.Value)
    {
        Plans = GetCollection<Domain.Entities.Plan>(nameof(Plans).Underscore());
        PlanSubscriptions = GetCollection<PlanSubscription>(nameof(PlanSubscriptions).Underscore());
    }

    public IMongoCollection<Domain.Entities.Plan> Plans { get; }
    public IMongoCollection<PlanSubscription> PlanSubscriptions { get; }
}
