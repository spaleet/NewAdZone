using MongoDB.Driver;
using Ticket.Application.Exceptions;
using Ticket.Domain.Enums;

namespace Ticket.Application.Features.User.PostingMessage;
public record PostMessage(string UserId, string TicketId, string Text) : ICommand;

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
            .MinLengthValidator("متن", 50)
            .MaxLengthValidator("متن", 1000);
    }
}

public class PostMessageHandler : ICommandHandler<PostMessage>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public PostMessageHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PostMessage request, CancellationToken cancellationToken)
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

        return Unit.Value;
    }
}
