using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ad.Infrastructure;

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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }
}
