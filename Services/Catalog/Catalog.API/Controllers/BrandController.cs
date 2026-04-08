using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{

    public class BrandController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
