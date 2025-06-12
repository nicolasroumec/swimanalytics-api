using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using PetAPI.Models.DTOs;
using swimanalytics.Models.DTOs;
using swimanalytics.Services.Implementations;
using swimanalytics.Services.Interfaces;
using swimanalytics.Models.Responses;

namespace swimanalytics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("changePassword")]
        public ActionResult<AnyType> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            Response response = new Response();

            try
            {
                response = _userService.ChangePassword(model);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [Authorize]
        [HttpPost("changePhone")]
        public ActionResult<AnyType> ChangePhone([FromBody] ChangePhoneDTO model)
        {
            Response response = new Response();

            try
            {
                string email = User.FindFirst("Account") != null ? User.FindFirst("Account").Value : string.Empty;

                response = _userService.ChangePhone(model, email);

                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpPost("forgotPassword")]
        public ActionResult<AnyType> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            Response response = new Response();
            try
            {
                response = _userService.ForgotPassword(model);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpGet("getAll")]
        public ActionResult GetAll() 
        {
            Response response = new Response();

            try
            {
                response = _userService.GetAll();
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpGet("getByEmail")]
        public ActionResult GetByEmail(string email)
        {
            Response response = new Response();

            try
            {
                response = _userService.GetByEmail(email);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AnyType>> Register([FromBody] RegisterDTO model)
        {
            Response response = new Response();

            try
            {
                response = await _userService.Register(model);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpPost("resendCode")]
        public async Task<ActionResult<AnyType>> ResendVerificationCode([FromBody] ResendVerificationCodeDTO model)
        {
            Response response = new Response();

            try
            {
                response = await _userService.ResendVerificationCode(model.email);
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [Authorize]
        [HttpPost("updateProfile")]
        public ActionResult<AnyType> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            Response response = new Response();

            try
            {
                string email = User.FindFirst("Account") != null ? User.FindFirst("Account").Value : string.Empty;

                response = _userService.UpdateProfile(model, email);

                return new JsonResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = e.Message;
                return new JsonResult(response);
            }
        }

        [HttpPost("verify")]
        public ActionResult<AnyType> VerifyAccount([FromBody] VerifyAccountDTO model)
        {
            Response response = new Response();

            try
            {
                response = _userService.VerifyAccount(model.email, model.code);
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
