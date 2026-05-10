using Asp.Versioning;
using Catalog.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2.0")]
    public class TestV2Controller : BaseApiController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("done");
        }
    }
}
