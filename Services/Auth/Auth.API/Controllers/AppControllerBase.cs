using Auth.Application.Bases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class AppControllerBase : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IActionResult Result<T>(Response<T> response)
    {
        return response.StatusCode switch
        {
            200 => Ok(response),

            201 => Created(string.Empty, response),

            204 => NoContent(),

            400 => BadRequest(response),

            401 => Unauthorized(response),

            403 => StatusCode(StatusCodes.Status403Forbidden, response),

            404 => NotFound(response),

            409 => Conflict(response),

            422 => UnprocessableEntity(response),

            500 => StatusCode(StatusCodes.Status500InternalServerError, response),

            _ => StatusCode(response.StatusCode, response)
        };
    }
}