using AutoMapper;
using BuildingBlocks.Core.Models.Paging;
using BuildingBlocks.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Ticket.Application.Dtos;
using Ticket.Application.Extensions;

namespace Ticket.Application.Features.User.GettingTickets;

public record GetTicketsUser(string UserId, string? Search) : PagingQuery<GetTicketsUserResponse>;

public class GetTicketsUserValidator : AbstractValidator<GetTicketsUser>
{
    public GetTicketsUserValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه");
    }
}

public class GetTicketsUserHandler : IQueryHandler<GetTicketsUser, GetTicketsUserResponse>
{
    private readonly TicketDbContext _context;
    private readonly IMapper _mapper;

    public GetTicketsUserHandler(TicketDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTicketsUserResponse> Handle(GetTicketsUser request, CancellationToken cancellationToken)
    {
        var query = _context.Tickets.AsQueryable()
                                        .Where(x => x.UserId == request.UserId)
                                        .OrderByDescending(x => x.CreationDate)
                                        .AsQueryable();

        // search by title
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Title.Contains(request.Search));

        // apply paging
        var tickets = query.ApplyPagingSync<Domain.Entities.Ticket, TicketDto>(
                                                                          _mapper.ConfigurationProvider,
                                                                          request.Page,
                                                                          request.PageSize);
        tickets.MapTicketsDate();

        return new GetTicketsUserResponse(tickets);
    }
}