using ToDo.API;

namespace ToDo.Core
{
    public class TaskController : ITaskController, ITaskFactory
    {
        private readonly ITaskStorage _taskStorage;

        public TaskController(ITaskStorage taskStorage)
        {
            _taskStorage = taskStorage;
        }

        public Task<bool> AddTask(string description)
        {
            throw new NotImplementedException();
        }

        public IToDoTask CreateToDoTask(long id, string description, bool hasCompleted)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IToDoTask>> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}