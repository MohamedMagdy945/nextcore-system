using Asp.Versioning;
using Catalog.Application.Features.Commands.CreateProduct;
using Catalog.Application.Features.Commands.DeleteProduct;
using Catalog.Application.Features.Commands.UpdateProduct;
using Catalog.Application.Features.Queries.GetAllProducts;
using Catalog.Application.Features.Queries.GetAllProductsByName;
using Catalog.Application.Features.Queries.GetProductById;
using Catalog.Application.Responses;
using Catalog.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductsController : AppControllerBase
    {

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductsByName")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetProductsByName(string productName)
        {
            var query = new GetAllProductsByNameQuery(productName);
            var result = await Mediator.Send(query);
            _logger.LogInformation($"Product with {productName} ({result})");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> GetAllProducts([FromQuery] ProductParams specs)
        {
            var query = new GetAllProductsQuery(specs);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> CreateProduct(CreateProductCommand productCommand)
        {
            var result = await Mediator.Send(productCommand);
            return Ok(result);
        }


        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> UpdateProduct(UpdateProductCommand productCommand)
        {
            var result = await Mediator.Send(productCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductResponseDto>>> DeleteProduct(string id)
        {
            var productCommand = new DeleteProductCommand(id);
            var result = await Mediator.Send(productCommand);
            return Ok(result);
        }

    }
}
