using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Plan.Infrastructure.Context;

namespace Plan.Infrastructure.Seed;

public class PlanDbInitializer
{
    private readonly PlanDbContext _context;
    private readonly ILogger<PlanDbInitializer> _logger;

    public PlanDbInitializer(ILogger<PlanDbInitializer> logger, PlanDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            bool seedExists = _context.Plans.Find(_ => true).Any();

            if (!seedExists)
            {
                List<Domain.Entities.Plan> seeds = new List<Domain.Entities.Plan>
                {
                    new("BasicPlan", 10, 0),
                    new("PremiumPlan", 50, 100000)
                };

                await _context.Plans.InsertManyAsync(seeds);
            }

            _logger.LogInformation("Successfully initialized Plan Db!");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }
}