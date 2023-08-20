using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.User.PostingMessage;
using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Api.Controllers.User;

public class UserTicketController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> PostTicket([FromBody] PostTicket request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPost("message")]
    public async Task<IActionResult> PostMessage([FromBody] PostMessage request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }
}
