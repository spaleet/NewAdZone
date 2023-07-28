using Ad.Application.Features.Ad.GettingAd;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class AdController : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
    {
        var ad = await Mediator.Send(new GetAd(id), cancellationToken);

        return Ok(ad);
    }
}
