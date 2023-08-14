using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Application.Features.AdGallery.GettingGallery;
using Ad.Application.Features.AdGallery.RemovingGallery;
using Ad.Application.Features.AdGallery.UploadingGallery;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class GalleryController : BaseController
{
    private readonly IWebHostEnvironment _environment;

    public GalleryController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        var galleryId = Guid.Parse(id);

        var res = await Mediator.Send(new GetGallery(galleryId), cancellationToken);

        string webRoot = _environment.WebRootPath;
        string path = Path.Combine(webRoot, $"upload/ad_gallery/{res.ImageSrc}");

        return PhysicalFile(path, res.ContentType);
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
