using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Exceptions;

namespace Ticket.Application.Features.Admin.GettingDetails;

public record GetTicketDetailsAdmin(string TicketId) : IQuery<GetTicketDetailsAdminResponse>;

public class GetAdValidator : AbstractValidator<GetTicketDetailsAdmin>
{
    public GetAdValidator()
    {
        RuleFor(x => x.TicketId)
            .RequiredValidator("شناسه");
    }
}


public class GetTicketDetailsAdminHandler : IQueryHandler<GetTicketDetailsAdmin, GetTicketDetailsAdminResponse>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public GetTicketDetailsAdminHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTicketDetailsAdminResponse> Handle(GetTicketDetailsAdmin request, CancellationToken cancellationToken)
    {
        var ticket = _context.Tickets.AsQueryable().FirstOrDefault(x => x.Id == request.TicketId);

        if (ticket is null)
            throw new InvalidTicketException();

        var messages = await _context.TicketMessages
            .Find(x => x.TicketId == ticket.Id)
            .ToListAsync();

        var result = _mapper.Map(ticket, new GetTicketDetailsAdminResponse());

        result.Messages = messages.Select(x => _mapper.Map(x, new TicketMessageDto()))
                                  .ToArray();

        return result;
    }
}
