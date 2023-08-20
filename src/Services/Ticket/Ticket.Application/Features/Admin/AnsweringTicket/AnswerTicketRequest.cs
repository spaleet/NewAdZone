namespace Ticket.Application.Features.Admin.AnsweringTicket;

public record AnswerTicketRequest(string TicketId, string Text);