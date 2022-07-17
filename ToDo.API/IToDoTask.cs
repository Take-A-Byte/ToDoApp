namespace ToDo.API
{
    public interface IToDoTask
    {
        long Id { get; }
        string Description { get; set; }
        bool HasCompleted { get; set; }
    }
}