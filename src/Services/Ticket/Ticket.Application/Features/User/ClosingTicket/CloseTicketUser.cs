using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.ClosingTicket;

public record CloseTicketUser : ICommand
{
    [JsonIgnore]
    [BindNever]
    public string UserId { get; set; }

    public string TicketId { get; set; }
}

public class CloseTicketUserValidator : AbstractValidator<CloseTicketUser>
{
    public CloseTicketUserValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه کاربری");

        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");
    }
}

public class CloseTicketUserHandler : ICommandHandler<CloseTicketUser>
{
    private readonly TicketDbContext _context;

    public CloseTicketUserHandler(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CloseTicketUser request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null || ticket.UserId != request.UserId)
            throw new InvalidTicketException();

        ticket.State = Domain.Enums.TicketStateEnum.Closed;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return Unit.Value;
    }
}