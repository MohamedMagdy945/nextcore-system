using Asp.Versioning;
using Catalog.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class TestController : BaseApiController
    {
        [HttpGet]

        public IActionResult Index()
        {
            return Ok("dibe");
        }
    }
}
