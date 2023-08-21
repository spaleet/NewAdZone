using BuildingBlocks.Core.Models.Paging;
using Ticket.Application.Dtos;

namespace Ticket.Application.Features.User.GettingTickets;

public record GetTicketsUserResponse(PagingModel<TicketDto> Ads);
