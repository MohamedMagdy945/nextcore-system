using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductsByName")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetProductsByName(string productName)
        {
            var query = new GetAllProductsByNameQuery(productName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("GreateProduct")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GreateProduct(CreateProductCommand productCommand)
        {
            var result = await _mediator.Send(productCommand);
            return Ok(result);
        }
    }
}
