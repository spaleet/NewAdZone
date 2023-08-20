using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.User.GettingDetails;

public record GetTicketDetailsUser(string TicketId, string UserId) : IQuery<GetTicketDetailsUserResponse>;

public class GetAdValidator : AbstractValidator<GetTicketDetailsUser>
{
    public GetAdValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");

        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه");
    }
}


public class GetTicketDetailsUserHandler : IQueryHandler<GetTicketDetailsUser, GetTicketDetailsUserResponse>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public GetTicketDetailsUserHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTicketDetailsUserResponse> Handle(GetTicketDetailsUser request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null || ticket.UserId != request.UserId)
            throw new InvalidTicketException();

        var messages = await _context.TicketMessages
            .Find(x => x.TicketId == ticket.Id)
            .ToListAsync();

        var result = _mapper.Map<GetTicketDetailsUserResponse>(ticket);

        result.Messages = messages.Select(x => _mapper.Map(x, new TicketMessageDto()))
                                  .ToArray();

        return result;
    }
}
