using BuildingBlocks.Core.Exceptions.Base;
using BuildingBlocks.Core.Exceptions.ExpandedProblemDetails;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Core.Web.Extenions;

public static class ErrorHandlingExtensions
{
    public static void AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(x =>
        {
            x.ShouldLogUnhandledException = (httpContext, exception, problemDetails) =>
            {
                var env = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment() || env.IsStaging();
            };

            // Control when an exception is included
            // Include in development, etc
            //x.IncludeExceptionDetails = (ctx, _) =>
            //{
            //    // Fetch services from HttpContext.RequestServices
            //    var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
            //    return env.IsDevelopment() || env.IsStaging();
            //};

            // Don't show exception details
            x.IncludeExceptionDetails = (ctx, _) =>
            {
                return false;
            };

            x.Map<ConflictException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = StatusCodes.Status409Conflict,
                        Detail = ex.Message,
                        Type = "https://somedomain/application-rule-validation-error"
                    }
            );

            // TODO : Add FluentValidation
            // Exception will produce and returns from our FluentValidation RequestValidationBehavior
            //x.Map<ValidationException>(
            //    ex =>
            //        new ProblemDetails
            //        {
            //            Title = ex.GetType().Name,
            //            Status = StatusCodes.Status400BadRequest,
            //            Detail = JsonConvert.SerializeObject(ex.ValidationResultModel.Errors),
            //            Type = "https://somedomain/input-validation-rules-error"
            //        }
            //);

            x.Map<BadRequestException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = ex.Message,
                        Type = "https://somedomain/bad-request-error",
                    }
            );

            x.Map<ArgumentException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = ex.Message,
                        Type = "https://somedomain/argument-error"
                    }
            );
            x.Map<NotFoundException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = (int)ex.StatusCode,
                        Detail = ex.Message,
                        Type = "https://somedomain/not-found-error"
                    }
            );
            x.Map<ApiException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = (int)ex.StatusCode,
                        Detail = ex.Message,
                        Type = "https://somedomain/api-server-error"
                    }
            );
            x.Map<AppException>(
                ex =>
                    new ProblemDetails
                    {
                        Title = ex.GetType().Name,
                        Status = (int)ex.StatusCode,
                        Detail = ex.Message,
                        Type = "https://somedomain/application-error"
                    }
            );
            x.Map<ForbiddenException>(ex => new ForbiddenProblemDetails(ex.Message));
            x.Map<UnAuthorizedException>(ex => new UnauthorizedProblemDetails(ex.Message));
            x.Map<IdentityException>(ex =>
            {
                var pd = new ProblemDetails
                {
                    Status = (int)ex.StatusCode,
                    Title = ex.GetType().Name,
                    Detail = ex.Message,
                    Type = "https://somedomain/identity-error"
                };

                return pd;
            });

            x.Map<HttpRequestException>(ex =>
            {
                var pd = new ProblemDetails
                {
                    Status = (int?)ex.StatusCode,
                    Title = ex.GetType().Name,
                    Detail = ex.Message,
                    Type = "https://somedomain/http-error"
                };

                return pd;
            });

            x.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
            x.MapStatusCode = context => new StatusCodeProblemDetails(context.Response.StatusCode);
        });
    }
}
