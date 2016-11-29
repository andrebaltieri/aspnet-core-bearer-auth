using DomainNotificationHelperCore.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Balta.io.Api.Controllers
{
    public class BaseController : Controller
    {
        public async Task<IActionResult> ReturnResponse(ServerCommand service, object success, object error)
        {
            if (service.HasNotifications())
                return BadRequest(new { success = false, data = error, errors = service.GetNotifications() });
           
            return Ok(new { success = true, data = success });
        }
    }
}
