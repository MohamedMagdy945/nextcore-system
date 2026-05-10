using Asp.Versioning;
using Catalog.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiVersion("2")]
    public class ProductsV2Controller : BaseApiController
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("test");
        }
    }
}
