namespace ToDo.API
{
    public interface ITaskFactory
    {
        IToDoTask CreateToDoTask(long id, string description, bool hasCompleted);
    }
}
