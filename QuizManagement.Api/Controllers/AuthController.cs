using QuizManagement.Shared.Data;
using QuizManagement.Shared.Models;
using QuizManagement.Api.Models;
using QuizManagement.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private IJwtUtils _jwtUtils;

        public AuthController(IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO data)
        {
            return Ok(await _userRepository.Login(data));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO data)
        {
            return Ok(await _userRepository.Register(data));
        }

        [HttpGet("Verify")]
        public ActionResult Verify()
        {
            var user = (User)HttpContext.Items["User"]!;
            return Ok(user);
        }

        [HttpPost("Change-Password")]
        public async Task<ActionResult> ChangePassword(PasswordDTO data)
        {
            var user = (User)HttpContext.Items["User"]!;

            return Ok(await _userRepository.ChangePassword(user.Id, data));
        }
    }
}
