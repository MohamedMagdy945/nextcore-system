using Asp.Versioning;
using Catalog.Application.Features.Queries.GetAllTypes;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("1.0")]
    public class TypeController : AppControllerBase
    {
        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
