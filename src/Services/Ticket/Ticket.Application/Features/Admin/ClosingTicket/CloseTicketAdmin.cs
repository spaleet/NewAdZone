using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.Admin.ClosingTicket;

public record CloseTicketAdmin(string TicketId) : ICommand;


public class CloseTicketAdminValidator : AbstractValidator<CloseTicketAdmin>
{
    public CloseTicketAdminValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");
    }
}

public class CloseTicketAdminHandler : ICommandHandler<CloseTicketAdmin>
{
    private readonly TicketDbContext _context;
    public CloseTicketAdminHandler(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CloseTicketAdmin request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null)
            throw new InvalidTicketException();

        ticket.State = Domain.Enums.TicketStateEnum.Closed;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return Unit.Value;
    }
}
