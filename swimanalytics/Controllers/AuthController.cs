using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using swimanalytics.Models.DTOs;
using swimanalytics.Models.Entities;
using swimanalytics.Models.Enums;
using swimanalytics.Repositories.Implementations;
using swimanalytics.Repositories.Interfaces;
using swimanalytics.Services.Interfaces;
using testpush.Models.Responses;

namespace swimanalytics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("Login")]
        public ActionResult<AnyType> Login([FromBody] LoginDTO model)
        {
            Response response = new Response();

            try
            {
                if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
                {
                    response.statusCode = 401;
                    response.message = "Empty fields";
                    return new JsonResult(response);
                }

                User user = _userRepository.GetByEmail(model.Email);

                response = _authService.Login(model, user);

                if (response.statusCode != 200)
                    return new JsonResult(response);

                string token = _authService.MakeToken(user.Email, user.Role, 15);

                response = new ResponseModel<string>(200, "Ok", token);

                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }
    }
}
