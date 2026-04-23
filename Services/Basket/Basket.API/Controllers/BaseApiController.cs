using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediatorInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>()!;

    }
}
