using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Webshop.Repos;
using Webshop.Repos.Models;
using Webshop.Shared.DTOs;

namespace Webshop.Services
{
    public class UserService
    {
        private readonly EmailService _emailService;
        private readonly HashingService _hashingService;
        private readonly IUserRepository _userRepository;
        private readonly ValidationService _validationService;
        private readonly RateLimitingService _rateLimitingService;
        private readonly PwnedPasswordService _pwnedPasswordService;

        public UserService(
            EmailService emailService,
            HashingService hashingService,
            IUserRepository userRepository,
            ValidationService validationService,
            RateLimitingService rateLimitingService,
            PwnedPasswordService pwnedPasswordService
            )
        {
            _emailService = emailService;
            _hashingService = hashingService;
            _userRepository = userRepository;
            _validationService = validationService;
            _rateLimitingService = rateLimitingService;
            _pwnedPasswordService = pwnedPasswordService;
        }

        public async Task<User> RegisterUserAsync(UserAuthDto userAuthDto)
        {
            userAuthDto.Email = userAuthDto.Email.Trim().ToLower();

            if (!_validationService.IsEmailValid(userAuthDto.Email))
            {
                throw new InvalidOperationException("Invalid email format.");
            }

            if (!_validationService.IsPasswordValidLength(userAuthDto.Password))
            {
                throw new InvalidOperationException("Password not strong enough.");
            }

            if (!_validationService.IsPasswordStrong(userAuthDto.Password))
            {
                throw new InvalidOperationException("Password not strong enough");
            }

            if (await _pwnedPasswordService.IsPasswordPwned(userAuthDto.Password))
            {
                throw new InvalidOperationException("This password has been found in data breaches. Please choose another.");
            }

            var passwordHash = _hashingService.GenerateHash(userAuthDto.Password);
            var createdUser = new User
            {
                Email = userAuthDto.Email,
                PasswordHash = passwordHash
            };

            var addedUser = await _userRepository.AddAsync(createdUser);
            return addedUser;
        }

        public async Task ResetPasswordAsync(HttpContext httpContext, ResetPasswordDto resetPasswordDto)
        {
            string rateLimitKey = RateLimitingService.GenerateRateLimitKey(httpContext, resetPasswordDto.VisitorId);

            var hashedToken = _hashingService.ComputeSha256Hash(resetPasswordDto.Token);
            var user = await _userRepository.GetUserByPasswordResetTokenAsync(hashedToken);

            if (user == null || user.PasswordResetTokenExpiration < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Invalid or expired token.");
            }

            if (!_validationService.IsPasswordValidLength(resetPasswordDto.NewPassword) ||
                !_validationService.IsPasswordStrong(resetPasswordDto.NewPassword))
            {
                throw new InvalidOperationException("Password does not meet the required criteria.");
            }

            user.PasswordHash = _hashingService.GenerateHash(resetPasswordDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;
            await _userRepository.UpdateAsync(user);

            _rateLimitingService.ResetAttempts(rateLimitKey, "PasswordReset");

            if (user.Email != null)
            {
                await _emailService.SendPasswordChangedNotification(user.Email);
            }
        }

        public async Task LoginAsync(HttpContext httpContext, UserAuthDto userAuthDto)
        {
            string rateLimitKey = RateLimitingService.GenerateRateLimitKey(httpContext, userAuthDto.VisitorId);

            if (_rateLimitingService.IsRateLimited(rateLimitKey, "Login"))
            {
                throw new InvalidOperationException();
            }

            bool isValidUser = await VerifyUserCredentialsAsync(userAuthDto.Email, userAuthDto.Password);
            if (!isValidUser)
            {
                _rateLimitingService.RegisterAttempt(rateLimitKey, "Login");
                throw new UnauthorizedAccessException();
            }

            _rateLimitingService.ResetAttempts(rateLimitKey, "Login");
        }

        public async Task<bool> VerifyUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.PasswordHash == null)
            {
                return false;
            }

            return _hashingService.VerifyHash(password, user.PasswordHash);
        }

        private async Task<string> GenerateAndSavePasswordResetTokenAsync(User user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hashedToken = _hashingService.ComputeSha256Hash(token);

            await _userRepository.SavePasswordResetTokenAsync(user.Id, hashedToken, DateTime.UtcNow.AddMinutes(30));

            return token;
        }

        public async Task ForgotPasswordAsync(HttpContext httpContext, ForgotPasswordDto forgotPasswordDto, string resetLink)
        {
            string rateLimitKey = RateLimitingService.GenerateRateLimitKey(httpContext, forgotPasswordDto.VisitorId);
            if (_rateLimitingService.IsRateLimited(rateLimitKey, "PasswordReset"))
            {
                throw new InvalidOperationException("Too many login attempts. Please try again later.");
            }

            var user = await _userRepository.GetUserByEmailAsync(forgotPasswordDto.Email);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                var token = await GenerateAndSavePasswordResetTokenAsync(user);
                await _emailService.SendPasswordResetEmail(user.Email, $"{resetLink}?token={token}");
                _rateLimitingService.RegisterAttempt(rateLimitKey, "PasswordReset");
            }
        }
    }
}
