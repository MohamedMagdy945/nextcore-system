using Asp.Versioning;
using Catalog.Application.Features.Queries.GetAllBrands;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("1.0")]
    public class BrandsController : AppControllerBase
    {
        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
