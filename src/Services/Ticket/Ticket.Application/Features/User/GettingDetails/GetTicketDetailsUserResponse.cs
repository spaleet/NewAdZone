using Ticket.Application.Dtos;
using Ticket.Domain.Enums;

namespace Ticket.Application.Features.User.GettingDetails;

public class GetTicketDetailsUserResponse
{
    public string Id { get; set; }

    public string Title { get; set; }

    public TicketDepartmentEnum Department { get; set; }

    public TicketPriorityEnum Priority { get; set; }

    public TicketStateEnum State { get; set; }

    public bool IsReadByAdmin { get; set; }

    public string SentDate { get; set; }

    // ignore mapping
    public TicketMessageDto[] Messages { get; set; }
}