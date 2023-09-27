using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.DeletingMessage;

public record DeleteMessage : ICommand
{
    public string MessageId { get; set; }

    [JsonIgnore]
    [BindNever]
    public string UserId { get; set; }
}

public class DeleteMessageValidator : AbstractValidator<DeleteMessage>
{
    public DeleteMessageValidator()
    {
        RuleFor(x => x.MessageId)
            .RequiredValidator("شناسه");

        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه کاربری");
    }
}

public class DeleteMessageHandler : ICommandHandler<DeleteMessage>
{
    private readonly TicketDbContext _context;
    public DeleteMessageHandler(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMessage request, CancellationToken cancellationToken)
    {
        var ticketMessage = await _context.TicketMessages.Find(x => x.Id == request.MessageId).FirstOrDefaultAsync();

        if (ticketMessage is null)
            throw new InvalidTicketException();

        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == ticketMessage.TicketId);

        if (ticket is null || ticket.UserId != request.UserId)
            throw new InvalidTicketException();

        var filter = Builders<TicketMessage>.Filter.Eq(x => x.Id, ticketMessage.Id);

        await _context.TicketMessages.DeleteOneAsync(filter);

        return Unit.Value;
    }
}
