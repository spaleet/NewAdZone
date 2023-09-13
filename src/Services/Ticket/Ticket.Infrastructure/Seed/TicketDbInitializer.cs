using Microsoft.Extensions.Logging;
using Ticket.Infrastructure.Context;

namespace Ticket.Infrastructure.Seed;

public class TicketDbInitializer
{
    private readonly TicketDbContext _context;
    private readonly ILogger<TicketDbInitializer> _logger;

    public TicketDbInitializer(ILogger<TicketDbInitializer> logger, TicketDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void Initialize()
    {
        try
        {
            _context.CheckIfAlive();

            _logger.LogInformation("Successfully initialized Ticket Db!");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }
}