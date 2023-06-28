using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Web;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}

[ApiController]
[Route("api/[controller]")]
public class BaseControllerLite : ControllerBase
{
}
