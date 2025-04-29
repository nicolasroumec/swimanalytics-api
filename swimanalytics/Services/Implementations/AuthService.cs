using Microsoft.IdentityModel.Tokens;
using swimanalytics.Models.DTOs;
using swimanalytics.Models.Entities;
using swimanalytics.Models.Enums;
using swimanalytics.Repositories.Implementations;
using swimanalytics.Repositories.Interfaces;
using swimanalytics.Services.Interfaces;
using swimanalytics.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using swimanalytics.Models.Responses;

namespace swimanalytics.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public Response Login(LoginDTO model, User user)
        {
            Response response = new Response();

            if (user == null)
            {
                response.statusCode = 401;
                response.message = "Invalid form";
                return response;
            }

            if (!(Encrypter.ValidateText(model.Password, user.Hash, user.Salt)))
            {
                response.statusCode = 401;
                response.message = "Wrong password";
                return response;
            }

            response.statusCode = 200;
            response.message = "Ok";
            return response;
        }

        public string MakeToken(string email, UserRole role, int minutes)
        {
            var claims = new[]
            {
                new Claim("Account", email),
                new Claim(ClaimTypes.Role, role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(minutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
