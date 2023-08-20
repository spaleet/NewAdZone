using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.Admin.AnsweringTicket;

public record AnswerTicket(string AdminId, string TicketId, string Text) : ICommand;

public class AnswerTicketValidator : AbstractValidator<AnswerTicket>
{
    public AnswerTicketValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");

        RuleFor(x => x.Text)
            .RequiredValidator("متن")
            .MinLengthValidator("متن", 50)
            .MaxLengthValidator("متن", 1000);
    }
}

public class AnswerTicketHandler : ICommandHandler<AnswerTicket>
{
    private readonly TicketDbContext _context;
    public AnswerTicketHandler(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AnswerTicket request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null)
            throw new InvalidTicketException();

        // insert ticket message
        var ticketMessage = new TicketMessage
        {
            TicketId = ticket.Id,
            Text = request.Text,
            UserId = request.AdminId
        };

        await _context.TicketMessages.InsertOneAsync(ticketMessage);

        ticket.State = Domain.Enums.TicketStateEnum.Answered;
        ticket.IsReadByAdmin = true;
        ticket.IsReadByUser = false;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return Unit.Value;
    }
}

