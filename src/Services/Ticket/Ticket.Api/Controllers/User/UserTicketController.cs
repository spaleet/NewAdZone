using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Api.Controllers.User;

public class UserTicketController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostTicket request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }
}
