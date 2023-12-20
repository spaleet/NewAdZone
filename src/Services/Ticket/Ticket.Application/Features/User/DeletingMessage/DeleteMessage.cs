using BuildingBlocks.Security.Utils;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.DeletingMessage;

public record DeleteMessage(string MessageId) : ICommand;

public class DeleteMessageValidator : AbstractValidator<DeleteMessage>
{
    public DeleteMessageValidator()
    {
        RuleFor(x => x.MessageId)
            .RequiredValidator("شناسه");
    }
}

public class DeleteMessageHandler : ICommandHandler<DeleteMessage>
{
    private readonly TicketDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public DeleteMessageHandler(TicketDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task<Unit> Handle(DeleteMessage request, CancellationToken cancellationToken)
    {
        string userId = _httpContext.HttpContext.User.GetUserId();

        var ticketMessage = await _context.TicketMessages.Find(x => x.Id == request.MessageId).FirstOrDefaultAsync();

        if (ticketMessage is null)
            throw new InvalidTicketException();

        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == ticketMessage.TicketId);

        if (ticket is null || ticket.UserId != userId)
            throw new InvalidTicketException();

        var filter = Builders<TicketMessage>.Filter.Eq(x => x.Id, ticketMessage.Id);

        await _context.TicketMessages.DeleteOneAsync(filter);

        return Unit.Value;
    }
}