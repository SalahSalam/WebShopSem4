using Microsoft.AspNetCore.Mvc;
using Webshop.Repos;
using Webshop.Services;
using Webshop.Shared.DTOs;

namespace Webshop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ValidationService _validationService;
        private readonly RateLimitingService _rateLimitingService;

        public UsersController(
            UserService userService,
            EmailService emailService,
            IUserRepository repository,
            ValidationService validationService,
            RateLimitingService rateLimitingService)
        {
            _userService = userService;
            _emailService = emailService;
            _userRepository = repository;
            _validationService = validationService;
            _rateLimitingService = rateLimitingService;
        }

        // GET api/<UsersController>/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/<UsersController>/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserAuthDto userAuthDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userResponse = await _userService.RegisterUserAsync(userAuthDto);
                return CreatedAtAction(nameof(GetUserById), new { id = userResponse.Id }, userResponse);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UsersController>/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> Login([FromBody] UserAuthDto userAuthDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.LoginAsync(HttpContext, userAuthDto);
                return Ok();
            }

            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, ex.Message);
            }

            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // POST api/<UsersController>/logout
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // TODO: Complete method
            return Ok(new { message = "Logged out" });
        }


        // POST api/<UsersController>/forgot-password
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string? resetLink = "https://127.0.0.1:5500/#/reset-password";
                if (resetLink == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                }

                await _userService.ForgotPasswordAsync(HttpContext, forgotPasswordDto, resetLink);
                return Ok("If this email exists in our system, you will receive a password reset email.");
            }

            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, ex.Message);
            }
        }

        // POST api/<UsersController>/reset-password
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.ResetPasswordAsync(HttpContext, resetPasswordDto);
                return Ok("Password has been reset successfully.");
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
