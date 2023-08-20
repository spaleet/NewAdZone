﻿using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.User.ClosingTicket;
using Ticket.Application.Features.User.DeletingMessage;
using Ticket.Application.Features.User.PostingMessage;
using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Api.Controllers.User;

public class UserTicketController : BaseController
{
    [HttpPost("post")]
    public async Task<IActionResult> PostTicket([FromBody] PostTicket request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        
        return Ok();
    }

    [HttpDelete("close")]
    public async Task<IActionResult> Close([FromBody] CloseTicketUser request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("message/post")]
    public async Task<IActionResult> PostMessage([FromBody] PostMessage request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpDelete("message/close")]
    public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessage request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }
}
