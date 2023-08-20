using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.DeletingAd;

public record DeleteTicket(string UserId, string TicketId) : ICommand;

public class DeleteTicketValidator : AbstractValidator<DeleteTicket>
{
    public DeleteTicketValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه کاربری");

        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");
    }
}

public class DeleteTicketHandler : ICommandHandler<DeleteTicket>
{
    private readonly TicketDbContext _context;
    public DeleteTicketHandler(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTicket request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null || ticket.UserId != request.UserId)
            throw new InvalidTicketException();

        ticket.State = Domain.Enums.TicketStateEnum.Closed;
        ticket.IsDeleted = true;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return Unit.Value;
    }
}
