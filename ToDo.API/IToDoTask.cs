namespace ToDo.API
{
    public interface IToDoTask
    {
        public string Description { get; set; }
        public bool HasCompleted { get; set; }
    }
}