namespace ToDo.API
{
    public interface ITaskController
    {
        Task<IReadOnlyList<IToDoTask>> GetAllTasks();
        Task<bool> AddTask(string description);
    }
}
