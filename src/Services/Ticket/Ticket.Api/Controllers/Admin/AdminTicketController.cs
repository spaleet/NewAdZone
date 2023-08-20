using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.Admin.AnsweringTicket;

namespace Ticket.Api.Controllers.Admin;

public class AdminTicketController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Answer([FromBody] AnswerTicketRequest request, CancellationToken cancellationToken)
    {
        var answerCommand = new AnswerTicket(request);

        answerCommand.AdminId = "10000"; // TODO Auth

        await Mediator.Send(answerCommand, cancellationToken);
        
        return Ok();
    }
}
