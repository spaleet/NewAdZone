using BuildingBlocks.Core.Web;
using BuildingBlocks.Security.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Dtos;
using Ticket.Application.Features.User.ClosingTicket;
using Ticket.Application.Features.User.DeletingMessage;
using Ticket.Application.Features.User.GettingDetails;
using Ticket.Application.Features.User.GettingTickets;
using Ticket.Application.Features.User.PostingMessage;
using Ticket.Application.Features.User.PostingTicket;

namespace Ticket.Api.Controllers.User;

[Authorize]
public class UserTicketController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetTicketsUserResponse>> Filter([FromQuery] GetTicketsUser request)
    {
        var tickets = await Mediator.Send(request);

        return Ok(tickets);
    }

    [HttpGet("{id}", Name = "GetTicketDetailsUser")]
    public async Task<ActionResult<GetTicketDetailsUserResponse>> GetTicketDetailsUser([FromRoute] string id, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new GetTicketDetailsUser(id, User.GetUserId()), cancellationToken);

        return Ok(res);
    }

    [HttpPost("post")]
    public async Task<ActionResult<TicketDto>> PostTicket([FromBody] PostTicket request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return CreatedAtRoute(nameof(GetTicketDetailsUser), new { id = res.Id, uId = res.UserId }, res);
    }

    [HttpDelete("close")]
    public async Task<IActionResult> Close([FromBody] CloseTicketUser request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("message/post")]
    public async Task<ActionResult<TicketDto>> PostMessage([FromBody] PostMessage request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return CreatedAtRoute(nameof(GetTicketDetailsUser), new { id = res.Id, uId = res.UserId }, res);
    }

    [HttpDelete("message/close")]
    public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessage request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }
}