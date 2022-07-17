namespace ToDo.API
{
    public interface ITaskController
    {
        Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks();
        Task<bool> AddTask(string description);
    }
}
