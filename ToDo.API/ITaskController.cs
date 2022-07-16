namespace ToDo.API
{
    public interface ITaskController
    {
        Task<IToDoTask> GetAllTasks();
        Task AddTask(string description);
    }
}
