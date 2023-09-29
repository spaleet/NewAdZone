using BuildingBlocks.Core.Models.Paging;
using BuildingBlocks.Core.Utilities;
using Ticket.Application.Dtos;

namespace Ticket.Application.Extensions;

public static class MapPagedTickets
{
    public static PagingModel<TicketDto> MapTicketsDate(this PagingModel<TicketDto> query)
    {
        // manually map SentDate because mongo doesnt support complex queries
        for (int i = 0; i < query.Items.Count; i++)
        {
            query.Items[i].SentDate = query.Items[i].CreationDate.ToLongShamsi();
        }

        return query;
    }
}