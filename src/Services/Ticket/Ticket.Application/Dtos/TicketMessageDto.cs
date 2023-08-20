namespace Ticket.Application.Dtos;

public record TicketMessageDto
{
    public string Id { get; set; }

    public string Text { get; set; }

    public string SentDate { get; set; }
}
