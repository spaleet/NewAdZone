using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Web;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseControllerParent : ControllerBase
{
    protected OkObjectResult Ok()
    {
        var res = new ApiResult(200, "عملیات با موفقیت انجام شد");

        return base.Ok(res);
    }

    protected OkObjectResult Ok(string message)
    {
        var res = new ApiResult(200, message);

        return base.Ok(res);
    }

    protected OkObjectResult Ok(object data)
    {
        var res = new ApiResult(200, "عملیات با موفقیت انجام شد", data);

        return base.Ok(res);
    }

    protected BadRequestObjectResult BadRequest()
    {
        var res = new ApiResult(400, "عملیات با خطا مواجه شد");

        return base.BadRequest(res);
    }

    protected BadRequestObjectResult BadRequest(string msg)
    {
        var res = new ApiResult(400, msg);

        return base.BadRequest(res);
    }

    protected BadRequestObjectResult BadRequest(object data)
    {
        var res = new ApiResult(400, "عملیات با خطا مواجه شد", data);

        return base.BadRequest(res);
    }
}

public abstract class BaseController : BaseControllerParent
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext?.RequestServices.GetService<IMediator>();
}

public class BaseControllerLite : BaseControllerParent
{
}
