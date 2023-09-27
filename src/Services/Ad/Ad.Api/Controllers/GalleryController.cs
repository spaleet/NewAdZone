using Ad.Application.Features.AdGallery.GettingGallery;
using Ad.Application.Features.AdGallery.RemovingGallery;
using Ad.Application.Features.AdGallery.UploadingGallery;
using Microsoft.AspNetCore.Authorization;

namespace Ad.Api.Controllers;

public class GalleryController : BaseController
{
    [HttpGet("{id}", Name = "GetGallery")]
    public async Task<IActionResult> GetGallery([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        var res = await Mediator.Send(new GetGallery(galleryId), cancellationToken);

        return PhysicalFile(res.ImagePath, res.ContentType);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UploadGalleryResponse>> Upload([FromForm] UploadGallery request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return CreatedAtRoute(nameof(GetGallery), new { id = res.Id }, res);
    }

    [HttpDelete("{id}/remove")]
    [Authorize]
    public async Task<IActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        await Mediator.Send(new RemoveGallery(galleryId), cancellationToken);
        return Ok();
    }
}
