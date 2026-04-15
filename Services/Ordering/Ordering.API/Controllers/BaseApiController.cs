using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediatorInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>()!;

    }
}
