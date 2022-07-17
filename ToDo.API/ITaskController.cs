namespace ToDo.API
{
    public interface ITaskController
    {
        Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks();
        Task<IToDoTask> AddTask(string description);
    }
}
