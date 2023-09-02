using BuildingBlocks.Core.Models.Paging;
using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Extensions;

namespace Ticket.Application.Features.Admin.GettingTickets;

public record GetTicketsAdmin(string? ByTitle, string? ByUserId) : PagingQuery<GetTicketsAdminResponse>;

public class GetTicketsAdminHandler : IQueryHandler<GetTicketsAdmin, GetTicketsAdminResponse>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public GetTicketsAdminHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTicketsAdminResponse> Handle(GetTicketsAdmin request, CancellationToken cancellationToken)
    {
        var query = _context.Tickets.AsQueryable()
            .OrderByDescending(x => x.CreationDate).AsQueryable();

        // search by userId
        if (!string.IsNullOrEmpty(request.ByUserId))
            query = query.Where(x => x.UserId == request.ByUserId);

        // search by title
        if (!string.IsNullOrEmpty(request.ByTitle))
            query = query.Where(x => x.Title.Contains(request.ByTitle));

        // apply paging
        var tickets = query.ApplyPagingSync<Domain.Entities.Ticket, TicketDto>(
                                                                          _mapper.ConfigurationProvider,
                                                                          request.Page,
                                                                          request.PageSize);
        tickets.MapTicketsDate();

        return new GetTicketsAdminResponse(tickets);
    }
}