using Ad.Application.Dtos;
using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Application.Features.AdCategory.GettingAdCategories;
using BuildingBlocks.Security;
using Microsoft.AspNetCore.Authorization;

namespace Ad.Api.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetAdCategoriesResponse>> Get(CancellationToken cancellationToken)
    {
        var categories = await Mediator.Send(new GetAdCategories(), cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Policy = AuthConsts.Admin)]
    public async Task<ActionResult<AdCategoryDto>> Create([FromBody] CreateAdCategory request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return Ok(res);
    }
}