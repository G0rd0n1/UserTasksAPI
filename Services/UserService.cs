using Microsoft.Data.SqlClient;
using System.Data;
using UserTasksAPI.Interfaces;
using UserTasksAPI.Models;

namespace UserTasksAPI.Services
{
    public class UserService : IUser
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public UserService()
        {
            // Empty constructor for test
        }

        public UserService(IConfiguration configuration, IDbConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
        }

        public virtual async Task<User> GetUserByID(int UserID)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();

                string query = @"
                    SELECT 
                        UserID, UserName, Email, Password
                    FROM 
                         UserTable
                    WHERE 
                        UserID = @UserID";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        var reader = await command.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                Tasks = new List<UserTasks>()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve user: " + ex.Message);
                }
                return null;
            }
        }

        public virtual async Task<User> GetUserByName(string UserName)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();

                string query = @"
                SELECT 
                    UserID, UserName, Email, Password
                FROM 
                    UserTable
                WHERE 
                    UserName = @UserName";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);
                        var reader = await command.ExecuteReaderAsync();

                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Email = reader.GetString(2),
                                Password = reader.GetString(3),
                                Tasks = new List<UserTasks>()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve user: " + ex.Message);
                }
                return null;
            }
        }

        public async Task<User> CreateANewUser(string UserName, string Email, string Password)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection"))) 
            { 
                await _connection.OpenAsync();

                string query = @"
                INSERT INTO UserTable 
                    (UserName, Email, Password)
                VALUES 
                    (@UserName, @Email, @Password)";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);
                        await command.ExecuteNonQueryAsync();
                    }

                    return new User
                    {
                        UserName = UserName,
                        Email = Email,
                        Password = Password,
                        Tasks = new List<UserTasks>()
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to create user: " + ex.Message);
                }
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();

                string query = @"
                UPDATE
                    UserTable
                SET
                    UserName = @UserName,
                    Email = @Email,
                    Password = @Password
                WHERE 
                    UserID = @UserID";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        await command.ExecuteNonQueryAsync();
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to update user: " + ex.Message);
                }
            }
        }

        public async Task<User> DeleteUser(int id)
        {
            using (var _connection = new SqlConnection(_configuration.GetConnectionString("DataBalkDBConnection")))
            {
                await _connection.OpenAsync();

                string query = @"
                DELETE FROM 
		            UserTable
                WHERE 
		            UserID = @UserID";

                try
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@UserID", id);
                        await command.ExecuteNonQueryAsync();
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete user: " + ex.Message);
                }
            }
        }
    }
}
