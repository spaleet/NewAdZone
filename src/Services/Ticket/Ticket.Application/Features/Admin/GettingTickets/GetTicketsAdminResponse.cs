using BuildingBlocks.Core.Models.Paging;
using Ticket.Application.Dtos;

namespace Ticket.Application.Features.Admin.GettingTickets;

public record GetTicketsAdminResponse(PagingModel<TicketDto> Tickets);