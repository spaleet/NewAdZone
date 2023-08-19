﻿using Humanizer;
using Ticket.Domain.Enums;

namespace Ticket.Application.Features.User.PostingTicket;

public record PostTicket(
    string UserId,
    string Title,
    TicketDepartmentEnum TicketSection,
    TicketPriorityEnum TicketPriority,
    string Text) : ICommand;

public class PostTicketValidator : AbstractValidator<PostTicket>
{
    public PostTicketValidator()
    {
        RuleFor(x => x.Title)
            .RequiredValidator("عنوان")
            .MaxLengthValidator("عنوان", 50);

        RuleFor(x => x.TicketSection)
            .RequiredValidator("بخش");

        RuleFor(x => x.TicketPriority)
            .RequiredValidator("اولویت");


        RuleFor(x => x.Text)
            .RequiredValidator("متن")
            .MinLengthValidator("متن", 50)
            .MaxLengthValidator("متن", 1000);
    }
}

public class PostTicketHandler : ICommandHandler<PostTicket>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public PostTicketHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PostTicket request, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map(request, new Domain.Entities.Ticket());

        ticket.State = TicketStateEnum.UnderProgress;
        ticket.IsReadByUser = true;

        // insert ticket
        await _context.Tickets.InsertOneAsync(ticket);

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