using System.Text.Json.Serialization;
using BuildingBlocks.Security.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.PostingMessage;

public record PostMessage(string TicketId, string Text) : ICommand<TicketDto>;

public class PostMessageValidator : AbstractValidator<PostMessage>
{
    public PostMessageValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");

        RuleFor(x => x.Text)
            .RequiredValidator("متن")
            .MinLengthValidator("متن", 15)
            .MaxLengthValidator("متن", 1000);
    }
}

public class PostMessageHandler : ICommandHandler<PostMessage, TicketDto>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContext;

    public PostMessageHandler(TicketDbContext context, IMapper mapper, IHttpContextAccessor httpContext)
    {
        _context = context;
        _mapper = mapper;
        _httpContext = httpContext;
    }

    public async Task<TicketDto> Handle(PostMessage request, CancellationToken cancellationToken)
    {
        string userId = _httpContext.HttpContext.User.GetUserId();

        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null || ticket.UserId != userId)
            throw new InvalidTicketException();

        // insert ticket message
        var ticketMessage = new TicketMessage
        {
            TicketId = ticket.Id,
            Text = request.Text,
            UserId = userId
        };

        await _context.TicketMessages.InsertOneAsync(ticketMessage);

        ticket.IsReadByAdmin = false;
        ticket.IsReadByUser = true;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return _mapper.Map<TicketDto>(ticket);
    }
}