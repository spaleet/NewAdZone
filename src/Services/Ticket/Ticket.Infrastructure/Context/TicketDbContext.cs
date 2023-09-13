
using System.Collections;
using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Ticket.Domain.Entities;

namespace Ticket.Infrastructure.Context;

public class TicketDbContext : MongoDbContext
{
    private readonly string TicketsName = nameof(Tickets).Underscore();
    private readonly string TicketMessagesName = nameof(TicketMessages).Underscore();

    public TicketDbContext(IOptions<MongoOptions> options) : base(options.Value)
    {
        Tickets = GetCollection<Domain.Entities.Ticket>(TicketsName);
        TicketMessages = GetCollection<TicketMessage>(TicketMessagesName);
    }

    public IMongoCollection<Domain.Entities.Ticket> Tickets { get; }
    public IMongoCollection<TicketMessage> TicketMessages { get; }

    public override void CheckIfAlive()
    {
        // Check Tickets Collection
        var ticketsFilter = new BsonDocument("name", TicketsName);
        bool ticketsExists = Database.ListCollectionNames(new ListCollectionNamesOptions { Filter = ticketsFilter }).Any();

        if (!ticketsExists)
        {
            Database.CreateCollection(TicketsName);
        }

        // Check TicketMessages Collection
        var ticketMessagesFilter = new BsonDocument("name", TicketMessagesName);
        bool ticketMessagesExists = Database.ListCollectionNames(new ListCollectionNamesOptions { Filter = ticketMessagesFilter }).Any();

        if (!ticketMessagesExists)
        {
            Database.CreateCollection(TicketMessagesName);
        }
    }
}
