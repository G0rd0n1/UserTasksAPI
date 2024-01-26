using System.Threading.Tasks;
using UserTasksAPI.Models;

namespace UserTasksAPI.Interfaces
{
    public interface ITask
    {
        public Task<UserTasks> CreateANewTask(UserTasks newTask);
        public Task<List<UserTasks>> GetTasksByUserID(int userID);
        public Task<UserTasks> UpdateTask(int taskID, UserTasks updatedTask);
        public Task DeleteTask(int taskID);
    }
}
