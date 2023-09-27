using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.PostingMessage;

public record PostMessage : ICommand<TicketDto>
{
    [JsonIgnore]
    [BindNever]
    public string UserId { get; set; }

    public string TicketId { get; set; }

    public string Text { get; set; }
}

public class PostMessageValidator : AbstractValidator<PostMessage>
{
    public PostMessageValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه کاربری");

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

    public PostMessageHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TicketDto> Handle(PostMessage request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);


        if (ticket is null || ticket.UserId != request.UserId)
            throw new InvalidTicketException();

        // insert ticket message
        var ticketMessage = new TicketMessage
        {
            TicketId = ticket.Id,
            Text = request.Text,
            UserId = request.UserId
        };

        await _context.TicketMessages.InsertOneAsync(ticketMessage);

        ticket.IsReadByAdmin = false;
        ticket.IsReadByUser = true;

        var filter = Builders<Domain.Entities.Ticket>.Filter.Eq(x => x.Id, ticket.Id);

        await _context.Tickets.ReplaceOneAsync(filter, ticket);

        return _mapper.Map<TicketDto>(ticket);
    }
}
