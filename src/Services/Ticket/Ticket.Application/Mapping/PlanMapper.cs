using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Application.Mapping;

public class TicketMapper : Profile
{
    public TicketMapper()
    {
        CreateMap<PostTicket, Domain.Entities.Ticket>();
    }
}
