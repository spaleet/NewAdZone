﻿using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Features.Ad.GettingRelatedAds;
using Ad.Application.Features.Ad.PostingAd;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class AdController : BaseController
{
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetAd([FromRoute] string slug)
    {
        var ad = await Mediator.Send(new GetAd(slug));

        return Ok(ad);
    }

    [HttpGet("{slug}/related")]
    public async Task<IActionResult> GetRelated([FromRoute] string slug)
    {
        var ads = await Mediator.Send(new GetRelatedAds(slug));

        return Ok(ads);
    }

    [HttpPost("post")]
    public async Task<IActionResult> Post([FromForm] PostAd postAd)
    {
        await Mediator.Send(postAd);

        return Ok("آگهی با موفقیت ساخته شد!");
    }
}