using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserTasksAPI.Models;
using UserTasksAPI.Services;

namespace UserTasksAPI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserName(string username)
        {
            var user = await _userService.GetUserByName(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("AddANewUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUser = await _userService.CreateANewUser(user.UserName, user.Email, user.Password);
                var jwt = _jwtService.GenerateJwt(createdUser.UserName);
                return CreatedAtAction("GetUser", new { id = createdUser.UserID }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create user" + ex);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _userService.GetUserByID(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.Password = updatedUser.Password;

                await _userService.UpdateUser(existingUser);
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to update user: " + ex);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userService.GetUserByID(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                await _userService.DeleteUser(id);
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to delete user: " + ex);
            }
        }
    }
}
