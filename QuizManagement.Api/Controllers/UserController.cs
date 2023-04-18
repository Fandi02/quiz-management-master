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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a all users
        /// </summary>
        [OnlyAdmin]
        [HttpGet()]
        public async Task<ActionResult> Users()
        {
            return Ok(await _userRepository.GetUsers());
        }

        /// <summary>
        /// Gets a specific User by id.
        /// </summary>
        [OnlyAdmin]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            return Ok(await _userRepository.GetUser(id));
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        [OnlyAdmin]
        [HttpPost]
        public async Task<ActionResult> AddUser(UserDTO data)
        {
            var user = new User
            {
                Name = data.Name,
                Username = data.Username,
                Email = data.Email,
                Password = data.Password,
                Role = Role.Admin,
            };

            return Ok(await _userRepository.AddUser(user));
        }

        /// <summary>
        /// Updates a User with a specific ID
        /// </summary>
        [OnlyAdmin]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserDTO data)
        {
            var user = await _userRepository.GetUser(id);

            user.Name = data.Name;
            user.Username = data.Username;
            user.Email = data.Email;
            user.Password = data.Password;

            return Ok(await _userRepository.UpdateUser(user));
        }

        /// <summary>
        /// Deletes a User with a specific Id.
        /// </summary>
        [OnlyAdmin]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return Ok(await _userRepository.DeleteUser(id));
        }
    }
}
