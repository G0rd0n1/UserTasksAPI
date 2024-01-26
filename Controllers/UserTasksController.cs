using Microsoft.AspNetCore.Mvc;
using UserTasksAPI.Interfaces;
using UserTasksAPI.Models;
using UserTasksAPI.Services;

namespace UserTasksAPI.Controllers
{
    public class UserTasksController : Controller
    {
        private readonly ITask _taskService;
        private readonly JwtService _jwtService;
        public UserTasksController(ITask taskService, JwtService jwtService)
        {
            _taskService = taskService;
            _jwtService = jwtService;
        }

        [HttpPost("CreateANewTask")]
        public async Task<IActionResult> CreateUserTask([FromBody] UserTasks newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTask = await _taskService.CreateANewTask(newTask);
                return CreatedAtAction("GetTask", new { id = createdTask.TaskID }, createdTask);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create task: " + ex);
            }
        }

        [HttpGet("GetTasksByUser/{userID}")]
        public async Task<IActionResult> GetTasksByUserID(int userID)
        {
            if (userID <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var tasks = await _taskService.GetTasksByUserID(userID);
                if (tasks.Count == 0)
                {
                    return NotFound("No tasks found for user.");
                }

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("UpdateTask/{taskID}")]
        public async Task<IActionResult> UpdateTask(int taskID, [FromBody] UserTasks updatedTask)
        {
            if (taskID <= 0)
            {
                return BadRequest("Invalid task ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring(7);
            }
            int currentUserID = _jwtService.GetUserIdFromToken(token); // Assuming you have a JWT service
            if (updatedTask.UserID != currentUserID)
            {
                return Unauthorized("You are not authorized to update this task.");
            }

            try
            {
                await _taskService.UpdateTask(taskID, updatedTask); // Use the same service for update
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to update task: " + ex.Message);
            }
        }

        [HttpDelete("DeleteTask/{taskID}")]
        public async Task<IActionResult> DeleteTask(int taskID)
        {
            try
            {
                await _taskService.DeleteTask(taskID);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to delete task: " + ex.Message);
            }
        }
    }
}
