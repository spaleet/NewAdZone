using Ad.Application.Features.AdCategory.GettingAdCategories;
using Ad.Infrastructure.Context;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers;

public class AdCategoryController : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetAdCategories(CancellationToken cancellationToken)
    {
        var categories = await Mediator.Send(new GetAdCategories(), cancellationToken);

        return Ok(categories);
    }
}
