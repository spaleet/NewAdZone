using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Application.Features.AdCategory.GettingAdCategories;

namespace Ad.Api.Controllers;

public class AdCategoryController : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var categories = await Mediator.Send(new GetAdCategories(), cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdCategory request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}
