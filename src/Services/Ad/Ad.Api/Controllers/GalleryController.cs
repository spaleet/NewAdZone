using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Application.Features.AdGallery.RemovingGallery;
using Ad.Application.Features.AdGallery.UploadingGallery;
using Azure.Core;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class GalleryController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadGallery request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        await Mediator.Send(new RemoveGallery(galleryId), cancellationToken);
        return Ok();
    }
}
