using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.Admin.AnsweringTicket;
using Ticket.Application.Features.Admin.ClosingTicket;

namespace Ticket.Api.Controllers.Admin;

public class AdminTicketController : BaseController
{
    [HttpPost("answer")]
    public async Task<IActionResult> Answer([FromBody] AnswerTicketRequest request, CancellationToken cancellationToken)
    {
        var answerCommand = new AnswerTicket(request);

        answerCommand.AdminId = "10000"; // TODO Auth

        await Mediator.Send(answerCommand, cancellationToken);
        
        return Ok();
    }

    [HttpDelete("close/{id}")]
    public async Task<IActionResult> Close([FromRoute] string id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CloseTicketAdmin(id), cancellationToken);
        
        return Ok();
    }
}
