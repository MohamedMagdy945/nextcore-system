using Auth.Application.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{

    public class TestController : AppControllerBase
    {

        [HttpGet("get")]
        public async Task<IActionResult> get()
        {

            return ApiResponse(Result<string>.Success("Test successful"));
        }


    }
}
