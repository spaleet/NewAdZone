using Ad.Application.Dtos;
using Ad.Application.Features.Ad.DeletingAd;
using Ad.Application.Features.Ad.EditingAd;
using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Features.Ad.GettingAds;
using Ad.Application.Features.Ad.GettingRelatedAds;
using Ad.Application.Features.Ad.PostingAd;

namespace Ad.Api.Controllers;

public class AdController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetAdsResponse>> GetAds([FromQuery] GetAds request)
    {
        var ads = await Mediator.Send(request);

        return Ok(ads);
    }

    [HttpGet("{slug}", Name = "GetAd")]
    public async Task<ActionResult<GetAdDetailsResponse>> GetAd([FromRoute] string slug)
    {
        var ad = await Mediator.Send(new GetAdDetails(slug));

        return Ok(ad);
    }

    [HttpGet("{slug}/related")]
    public async Task<ActionResult<List<AdDto>>> GetRelated([FromRoute] string slug)
    {
        var ads = await Mediator.Send(new GetRelatedAds(slug));

        return Ok(ads);
    }

    [HttpPost]
    public async Task<ActionResult<AdDto>> Post([FromForm] PostAd postAd)
    {
        var res = await Mediator.Send(postAd);

        return CreatedAtRoute(nameof(GetAd), new { slug = res.Slug }, res);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] EditAd editAd)
    {
        await Mediator.Send(editAd);

        return Ok("آگهی با موفقیت ویرایش شد!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        await Mediator.Send(new DeleteAd(id));

        return Ok("آگهی با موفقیت حذف شد!");
    }
}
