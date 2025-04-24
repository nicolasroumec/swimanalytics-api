using swimanalytics.Models.DTOs;
using swimanalytics.Models.Entities;
using swimanalytics.Repositories.Interfaces;
using swimanalytics.Services.Interfaces;
using swimanalytics.Tools;
using testpush.Models.Responses;

namespace swimanalytics.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Encrypter _encrypter;

        public UserService(
            IUserRepository userRepository, 
            Encrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public Response GetAll()
        {
            Response response = new Response();

            var users = _userRepository.GetAll().ToList();

            if(users == null || users.Count == 0) 
            {
                response.statusCode = 404;
                response.message = "Users not found";
                return response;
            }

            response = new ResponseCollection<User>(200, "Ok", users);
            return response;
        }

        public Response GetByEmail(string email) 
        {
            Response response = new Response();

            var user = _userRepository.GetByEmail(email);

            if(user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            response = new ResponseModel<User>(200, "Ok", user);
            return response;
        }

        public Response Register(RegisterDTO model) 
        {
            Response response = new Response();
            
            var validator = new RegisterDTOValidator();
            var result = validator.Validate(model);

            if(!result.IsValid)
            {
                response.statusCode = 400;
                response.message = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            var user = _userRepository.GetByEmail(model.Email);

            if(user != null) 
            {
                response.statusCode = 401;
                response.message = "Email already in use";
                return response;
            }

            Encrypter.EncryptString(model.Password, out byte[] hash, out byte[] salt);

            User newUser = new User();

            newUser.FirstName = model.FirstName;
            newUser.LastName = model.LastName; 
            newUser.Gender = model.Gender;
            newUser.Height = model.Height;
            newUser.Weight = model.Weight;
            newUser.Wingspan = model.Wingspan;
            newUser.Club = model.Club;
            newUser.Email = model.Email;
            newUser.Phone = model.Phone;
            newUser.Hash = hash;
            newUser.Salt = salt;
            newUser.IsActive = false;
            newUser.Role = Models.Enums.UserRole.Swimmer;

            _userRepository.Save(newUser);

            response.statusCode = 200;
            response.message = "Ok";
            return response;
        }
    }
}
