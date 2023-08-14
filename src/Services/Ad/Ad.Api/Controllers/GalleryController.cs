using Ad.Application.Features.AdGallery.GettingGallery;
using Ad.Application.Features.AdGallery.RemovingGallery;
using Ad.Application.Features.AdGallery.UploadingGallery;

namespace Ad.Api.Controllers;

public class GalleryController : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        var res = await Mediator.Send(new GetGallery(galleryId), cancellationToken);

        return PhysicalFile(res.ImagePath, res.ContentType);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadGallery request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        await Mediator.Send(new RemoveGallery(galleryId), cancellationToken);
        return Ok();
    }
}
