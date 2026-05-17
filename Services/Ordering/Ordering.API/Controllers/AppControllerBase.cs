using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    public class AppControllerBase : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    }
}
