using ToDo.API;

namespace ToDo.Core
{
    public class TaskController : ITaskController
    {
        public Task AddTask(string description)
        {
            throw new NotImplementedException();
        }

        public Task<IToDoTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}