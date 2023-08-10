using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Features.Ad.GettingRelatedAds;
using Ad.Application.Features.Ad.PostingAd;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class AdController : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAd([FromRoute] long id)
    {
        var ad = await Mediator.Send(new GetAd(id));

        return Ok(ad);
    }

    [HttpGet("{id}/related")]
    public async Task<IActionResult> GetRelated([FromRoute] long id)
    {
        var ads = await Mediator.Send(new GetRelatedAds(id));

        return Ok(ads);
    }

    [HttpPost("post")]
    public async Task<IActionResult> Post([FromForm] PostAd postAd)
    {
        await Mediator.Send(postAd);

        return Ok("آگهی با موفقیت ساخته شد!");
    }
}
