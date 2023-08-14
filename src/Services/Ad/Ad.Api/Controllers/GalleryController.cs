using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Application.Features.AdGallery.UploadingGallery;
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
}
