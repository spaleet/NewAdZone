using BuildingBlocks.Security.Utils;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.ClosingTicket;

public record CloseTicketUser(string TicketId) : ICommand;

public class CloseTicketUserValidator : AbstractValidator<CloseTicketUser>
{
    public CloseTicketUserValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");
    }
}

public class CloseTicketUserHandler : ICommandHandler<CloseTicketUser>
{
    private readonly TicketDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public CloseTicketUserHandler(TicketDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task<Unit> Handle(CloseTicketUser request, CancellationToken cancellationToken)
    {
        string userId = _httpContext.HttpContext.User.GetUserId();

        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null || ticket.UserId != userId)
            throw new InvalidTicketException();

        ticket.State = Domain.Enums.TicketStateEnum.Closed;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return Unit.Value;
    }
}