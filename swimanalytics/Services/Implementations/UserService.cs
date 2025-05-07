using swimanalytics.Models.DTOs;
using swimanalytics.Models.Entities;
using swimanalytics.Repositories.Implementations;
using swimanalytics.Repositories.Interfaces;
using swimanalytics.Services.Interfaces;
using swimanalytics.Tools;
using swimanalytics.Models.Responses;
using swimanalytics.Tools.Validators;
using FluentValidation;

namespace swimanalytics.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Encrypter _encrypter;
        private readonly IVerificationCodeRepository _verificationCodeRepository;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository userRepository, 
            Encrypter encrypter,
            IVerificationCodeRepository verificationCodeRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _verificationCodeRepository = verificationCodeRepository;
            _emailService = emailService;
        }

        public Response ChangePassword(ChangePasswordDTO model)
        {
            Response response = new Response();

            var validator = new ChangePasswordDTOValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                response.statusCode = 400;
                response.message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            var user = _userRepository.GetByEmail(model.Email);

            if (user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            Encrypter.EncryptString(model.Password, out byte[] hash, out byte[] salt);

            user.Hash = hash;
            user.Salt = salt;

            _userRepository.Save(user);

            response.statusCode = 200;
            response.message = "Ok";
            return response;
        }

        public Response ChangePhone(ChangePhoneDTO model, string email)
        {
            Response response = new Response();

            var user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            user.Phone = model.Phone;

            _userRepository.Save(user);

            response.statusCode = 200;
            response.message = "Ok";
            return response;
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

        public async Task<Response> Register(RegisterDTO model) 
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
            newUser.DateOfBirth = model.DateOfBirth;
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
            newUser.IsVerified = false;
            newUser.Role = Models.Enums.UserRole.Swimmer;

            _userRepository.Save(newUser);

            string verificationCode = VerificationCodeGenerator.GenerateRandomCode(6);

            VerificationCode code = new VerificationCode()
            {
                UserId = newUser.Id,
                Code = verificationCode,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(1),
                IsUsed = false
            };

            _verificationCodeRepository.Save(code);

            await _emailService.SendVerificationEmail(newUser.Email, verificationCode);

            response.statusCode = 200;
            response.message = "Registration successful. Please check your email to activate your account.";
            return response;
        }

        public async Task<Response> ResendVerificationCode(string email)
        {

            Response response = new Response();

            var user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            if (user.IsVerified)
            {
                response.statusCode = 400;
                response.message = "Account already verified";
                return response;
            }

            string verificationCode = VerificationCodeGenerator.GenerateRandomCode(6);

            VerificationCode code = new VerificationCode()
            {
                UserId = user.Id,
                Code = verificationCode,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(1),
                IsUsed = false
            };

            _verificationCodeRepository.Save(code);

            await _emailService.SendVerificationEmail(user.Email, verificationCode);

            response.statusCode = 200;
            response.message = "Verification code sent.";
            return response;
        }

        public Response UpdateProfile(UpdateProfileDTO model, string email) 
        {
            Response response = new Response();

            var validator = new UpdateProfileDTOValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                response.statusCode = 400;
                response.message = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            var user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Gender = model.Gender;
            user.Height = model.Height;
            user.Weight = model.Weight;
            user.Wingspan = model.Wingspan;
            user.Club = model.Club;

            _userRepository.Save(user);

            response.statusCode = 200;
            response.message = "Profile updated successfully";
            return response;
        }

        public Response VerifyAccount(string email, string code)
        {
            Response response = new Response();

            var user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                response.statusCode = 404;
                response.message = "User not found";
                return response;
            }

            var verificationCode = _verificationCodeRepository.GetByUserIdAndCode(user.Id, code);

            if (verificationCode == null)
            {
                response.statusCode = 400;
                response.message = "Invalid verification code";
                return response;
            }

            if (verificationCode.ExpiresAt < DateTime.Now)
            {
                response.statusCode = 400;
                response.message = "Verification code has expired";
                return response;
            }

            verificationCode.IsUsed = true;

            _verificationCodeRepository.Save(verificationCode);

            user.IsVerified = true;

            _userRepository.Save(user);

            response.statusCode = 200;
            response.message = "Account successfully verified";
            return response;
        }
    }
}
