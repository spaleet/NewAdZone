using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Features.Ad.GettingRelatedAds;
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

    [HttpGet("/{id}/related")]
    public async Task<IActionResult> GetRelated([FromRoute] long id, CancellationToken cancellationToken)
    {
        var ads = await Mediator.Send(new GetRelatedAds(id), cancellationToken);

        return Ok(ads);
    }
}
