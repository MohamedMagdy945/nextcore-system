using Asp.Versioning;
using Catalog.Application.Features.Queries.GetAllCategories;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("1.0")]
    public class CategoriesController : AppControllerBase
    {
        [HttpGet]
        [Route("GetAllCategories")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
