using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Balta.io.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public HomeController()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet]
        [Route("api/v1/home/user")]
        [Authorize(Policy = "User")]
        public IActionResult GetUser()
        {
            return Ok(new { data = "User" });
        }

        [HttpGet]
        [Route("api/v1/home/admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { data = "Admin" });
        }
    }
}
