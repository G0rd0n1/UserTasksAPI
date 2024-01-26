using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UserTasksAPI.Interfaces;
using UserTasksAPI.Models;

namespace UserTasksAPI.Services
{
    public class UserTasksService : ITask
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;

        public UserTasksService(IConfiguration configuration, IDbConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
        }

        public async Task<UserTasks> CreateANewTask(UserTasks newTask)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();

                string query = @"
                INSERT INTO 
                    TaskTable 
                        (Title, Description, UserID, DueDate)
                VALUES 
                        (@Title, @Description, @UserID, @DueDate)";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@Title", newTask.Title);
                        command.Parameters.AddWithValue("@Description", newTask.Description);
                        command.Parameters.AddWithValue("@UserID", newTask.UserID);
                        command.Parameters.AddWithValue("@DueDate", newTask.DueDate);
                        await command.ExecuteNonQueryAsync();
                        int newTaskId = (int)command.ExecuteScalar();
                        newTask.TaskID = newTaskId;
                        return newTask;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to create task: " + ex.Message);
                }
            }
        }

        public async Task<List<UserTasks>> GetTasksByUserID(int userID)
        {
            List<UserTasks> tasks = new List<UserTasks>();
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();
                string query = @"
                    SELECT
                        *
                    FROM
                        TaskTable
                    WHERE 
                        UserID = @UserID";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            UserTasks task = new UserTasks();
                            task.TaskID = reader.GetInt32("TaskID");
                            task.Title = reader.GetString("Title");
                            task.Description = reader.GetString("Description");
                            task.UserID = reader.GetInt32("UserID");
                            task.DueDate = reader.GetDateTime("DueDate");
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }

        public async Task<UserTasks> UpdateTask(int taskID, UserTasks updatedTask)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();
                string query = @"
                UPDATE 
                    TaskTable
                SET 
                    Title = @Title, Description = @Description, DueDate = @DueDate
                WHERE 
                    TaskID = @TaskID";

                await _connection.ExecuteAsync(query, new
                {
                    updatedTask.Title,
                    updatedTask.Description,
                    updatedTask.DueDate,
                    taskID
                });

                var updatedTaskData = await _connection.QueryFirstOrDefaultAsync<UserTasks>(
                    @"SELECT 
                        TaskID, Title, Description, DueDate
                    FROM 
                        TaskTable
                    WHERE 
                        TaskID = @TaskID",
                    new { taskID });
                return updatedTaskData;
            }
        }

        public async Task DeleteTask(int taskID)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();
                string query = @"DELETE FROM TaskTable WHERE TaskID = @TaskID";
                await _connection.ExecuteAsync(query, new { taskID });
            }
        }
    }
}
