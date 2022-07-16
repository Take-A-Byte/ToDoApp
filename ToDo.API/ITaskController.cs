namespace ToDo.API
{
    public interface ITaskController
    {
        Task<IList<IToDoTask>> GetAllTasks();
        Task<bool> AddTask(string description);
    }
}
