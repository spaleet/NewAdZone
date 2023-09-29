using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Plan.Domain.Entities;

namespace Plan.Infrastructure.Context;

public class PlanDbContext : MongoDbContext
{
    private readonly string PlansName = nameof(Plans).Underscore();
    private readonly string PlanSubscriptionsName = nameof(PlanSubscriptions).Underscore();

    public PlanDbContext(IOptions<MongoOptions> options) : base(options.Value)
    {
        Plans = GetCollection<Domain.Entities.Plan>(PlansName);
        PlanSubscriptions = GetCollection<PlanSubscription>(PlanSubscriptionsName);
    }

    public IMongoCollection<Domain.Entities.Plan> Plans { get; }
    public IMongoCollection<PlanSubscription> PlanSubscriptions { get; }

    public override void CheckIfAlive()
    {
        // Check Plans Collection
        var plansFilter = new BsonDocument("name", PlansName);
        bool plansExists = Database.ListCollectionNames(new ListCollectionNamesOptions { Filter = plansFilter }).Any();

        if (!plansExists)
        {
            Database.CreateCollection(PlansName);
        }

        // Check PlanSubscriptions Collection
        var planSubscriptionsFilter = new BsonDocument("name", PlanSubscriptionsName);
        bool planSubscriptionsExists = Database.ListCollectionNames(new ListCollectionNamesOptions { Filter = planSubscriptionsFilter }).Any();

        if (!planSubscriptionsExists)
        {
            Database.CreateCollection(PlanSubscriptionsName);
        }
    }
}