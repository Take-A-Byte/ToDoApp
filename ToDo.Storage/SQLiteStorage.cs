using ToDo.API;

namespace ToDo.Storage
{
    public class SQLiteStorage : ITaskStorage
    {
        public Task<bool> AddNewTask(long taskId, string description)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTask(long taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskCompleteness(long taskId, bool completeness)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskDescription(long taskId, string description)
        {
            throw new NotImplementedException();
        }
    }
}