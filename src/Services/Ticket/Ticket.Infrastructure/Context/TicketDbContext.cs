
using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticket.Domain.Entities;

namespace Ticket.Infrastructure.Context;

public class TicketDbContext : MongoDbContext
{
    public TicketDbContext(IOptions<MongoOptions> options) : base(options.Value)
    {
        Tickets = GetCollection<Domain.Entities.Ticket>(nameof(Tickets).Underscore());
        TicketMessages = GetCollection<TicketMessage>(nameof(TicketMessages).Underscore());
    }

    public IMongoCollection<Domain.Entities.Ticket> Tickets { get; }
    public IMongoCollection<TicketMessage> TicketMessages { get; }
}
