using UserTasksAPI.Models;

namespace UserTasksAPI.Interfaces
{
    public interface IUser
    {
        public Task<User> GetUserByID(int UserID);
        public Task<User> GetUserByName(string UserName);
        public Task<User> CreateANewUser(string UserName, string Email, string Password);
        public Task<User> UpdateUser(User user);
        public Task<User> DeleteUser(int id);

    }
}
