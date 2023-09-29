namespace Ticket.Application.Exceptions;

public class InvalidTicketException : BadRequestException
{
    public InvalidTicketException()
        : base("تیکت مورد نظر نامعتبر است.")
    {
    }
}