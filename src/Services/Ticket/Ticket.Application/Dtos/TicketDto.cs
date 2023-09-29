using System.Text.Json.Serialization;
using Ticket.Domain.Enums;

namespace Ticket.Application.Dtos;

public class TicketDto
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string Title { get; set; }

    public TicketDepartmentEnum Department { get; set; }

    public TicketPriorityEnum Priority { get; set; }

    public TicketStateEnum State { get; set; }

    public bool IsReadByUser { get; set; }

    public bool IsReadByAdmin { get; set; }

    [JsonIgnore]
    public DateTime CreationDate { get; set; }

    public string SentDate { get; set; }
}