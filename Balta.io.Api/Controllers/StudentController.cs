using Balta.io.Domain.Commands.StudentCommands;
using Balta.io.Domain.Commands.UserCommands;
using Balta.io.Domain.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Balta.io.Api.Controllers
{
    public class StudentController : BaseController
    {
        [HttpPost]
        [Route("api/v1/student")]
        public Task<IActionResult> RegisterUser([FromBody]RegisterStudentCommand student, [FromBody]RegisterUserCommand user)
        {
            RegisterStudentService service = new RegisterStudentService(student, user, null);
            service.Run();

            return ReturnResponse(
                service,
                new { message = "Bem vindo ao balta.io" },
                new { message = "Seu cadastro não pode ser efetuado" }
            );
        }
    }
}
