namespace ToDo.API
{
    public interface ITaskStorage
    {
        Task<bool> GetAllTasks();
        Task<bool> GetTask(long taskId);
        Task<bool> AddNewTask(long taskId, string description);
        Task<bool> UpdateTaskDescription(long taskId, string description);
        Task<bool> UpdateTaskCompleteness(long taskId, bool completeness);
    }
}
