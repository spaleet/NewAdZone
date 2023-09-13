using Ad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ad.Infrastructure.Seed;

public class AdDbInitializer
{
    private readonly AdDbContext _context;
    private readonly ILogger<AdDbInitializer> _logger;

    public AdDbInitializer(ILogger<AdDbInitializer> logger, AdDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Successfully initialized Ad Db!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }
}
