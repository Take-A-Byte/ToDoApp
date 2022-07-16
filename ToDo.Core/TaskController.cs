using ToDo.API;

namespace ToDo.Core
{
    public class TaskController : ITaskController
    {
        public Task<bool> AddTask(string description)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IToDoTask>> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}